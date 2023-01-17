import contentGetter from "./content-getter.js";
import arrayEquals from "../util/array-equals.js";
import urlFetcher from "../util/url-fetcher.js";

const generateNewContentKey = () => (Math.random() * 0xFFFFFF << 0).toString(16).padStart(6, '0'); // https://stackoverflow.com/questions/5092808/how-do-i-randomly-generate-html-hex-color-codes-using-javascript

const contentReferenceEquals = (a, b) => arrayEquals(a.keyValues, b.keyValues) && a.newContentKey == b.newContentKey && a.entityType == b.entityType;

const FIVE_MINUTES = 5 * 60 * 1000;

class StateManager {
  indexStorageKey = "cloudy:statesIndex";
  schema = "1.6";
  states = this.loadStates();

  loadStates() {
    let index = JSON.parse(localStorage.getItem(this.indexStorageKey) || JSON.stringify({ schema: this.schema, elements: [] }));

    if (index.schema != this.schema) {
      if (confirm(`Warning: The state schema has changed (new version: ${this.schema}, old version: ${index.schema}).\n\nThis means the format of local state has changed, and your local changes are no longer understood by the Admin UI.\n\nYou are required to clear your local changes to avoid any strange bugs.\n\nPress OK to continue, or cancel to do the necessary schema changes manually to your localStorage (not supported officially).`)) {
        Object.keys(localStorage)
          .filter(key => key.startsWith("cloudy:"))
          .forEach(key => localStorage.removeItem(key));

        return [];
      }
    }

    const result = [];

    for (let contentReference of index.elements) {
      result.push(JSON.parse(localStorage.getItem(`cloudy:${JSON.stringify(contentReference)}`), (key, value) => key == 'referenceDate' && value ? new Date(value) : value));
    }

    return result;
  }

  getAll() {
    return this.states.filter(state => this.hasChanges(state));
  }

  createStateForNewContent(entityType) {
    const contentReference = { newContentKey: generateNewContentKey(), keyValues: null, entityType };

    const state = {
      contentReference,
      referenceValues: {},
      referenceDate: new Date(),
      changes: [],
    };
    this.states.push(state);
    this.persist(state);

    return state;
  };

  createOrUpdateStateForExistingContent(contentReference, nameHint) {
    const existingState = this.getState(contentReference);
    if (existingState) {
      this.reloadContentForState(contentReference);
      return existingState;
    }

    const state = {
      contentReference,
      loading: true,
      nameHint,
      referenceValues: null,
      referenceDate: null,
      changes: [],
    };
    this.states.push(state);
    this.persist(state);

    this.loadContentForState(contentReference);

    return state;
  };

  reloadContentForState(contentReference) {
    let state = this.getState(contentReference);

    state = {
      ...state,
      loadingNewVersion: true,
      newVersion: null,
    };
    this.replace(state);

    contentGetter.get(contentReference).then(content => {
      state = this.getState(contentReference);

      if (JSON.stringify(state.referenceValues) == JSON.stringify(content)) {
        state = {
          ...state,
          loadingNewVersion: false,
        };
      } else {
        if (!this.hasChanges(state)) {
          state = {
            ...state,
            loadingNewVersion: false,
            referenceValues: content,
            referenceDate: new Date(),
          };
        } else {
          state = {
            ...state,
            loadingNewVersion: false,
            newVersion: {
              referenceValues: content,
              referenceDate: new Date(),
            },
          };
        }
      }

      this.replace(state);
    });
  }

  discardNewVersion(contentReference) {
    let state = this.getState(contentReference);

    state = {
      ...state,
      referenceValues: state.newVersion.referenceValues,
      referenceDate: state.newVersion.referenceDate,
      newVersion: null,
    };
    this.replace(state);
  }

  async loadContentForState(contentReference) {
    const content = await contentGetter.get(contentReference);

    let state = this.getState(contentReference);

    state = {
      ...state,
      loading: false,
      nameHint: null,
      referenceValues: content,
      referenceDate: new Date(),
      changes: [],
    };

    this.replace(state);
  }

  getOrCreateLatestChange(state, type, path) {
    let change = state.changes.find(c => c['$type'] == type && arrayEquals(path, c.path));

    if (!change || Date.now() - change.date > FIVE_MINUTES) {
      change = { '$type': type, 'date': Date.now(), path };
      state.changes.push(change);
    }

    return change;
  }

  async save(contentReferences) {
    const states = contentReferences.map(c => this.getState(c));
    const response = await urlFetcher.fetch("/Admin/api/form/entity/save", {
      credentials: "include",
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        changedEntities: states.map(state => ({
          entityReference: state.contentReference,
          entityChanges: state.changes,
        }))
      }),
    }, 'Could not save entity');

    const success = response.results.every(r => r.success);

    if (success) {
      notificationManager.addNotification((item) => item.setText('Entity has been saved.'));
    } else {
      response.results.filter(r => !r.success).forEach(result => {
        var errors = document.createElement("ul");
        Object.entries(result.validationErrors).forEach(error => {
          var item = document.createElement("li");
          item.innerText = `${error[0]}: ${error[1]}`;
          errors.append(item);
        });
        notificationManager.addNotification((item) => item.setText(`Error saving:`, errors));
      });
    }

    for (let result of results.filter(r => r.success)) {
      this.loadContentForState(result.contentReference);
    }
  }

  replace(state) {
    this.states[this.states.findIndex(s => contentReferenceEquals(s.contentReference, state.contentReference))] = state;
    this.persist(state);
  }

  remove(contentReference) {
    this.states.splice(this.states.findIndex(s => contentReferenceEquals(s.contentReference, contentReference)), 1);
    this.unpersist(contentReference);

    return contentReference;
  };

  getState(contentReference) {
    return this.states.find(s => contentReferenceEquals(s.contentReference, contentReference));
  }

  discardChanges(contentReference, change) {
    const state = this.getState(contentReference);

    state.changes.splice(0, state.changes.length);

    this.persist(state);
  }

  hasChanges(state, path = null) {
    if (path) {
      return state.changes?.find(c => arrayEquals(c.path, path));
    }

    return state.changes?.length;
  }

  updateIndex() {
    localStorage.setItem(this.indexStorageKey, JSON.stringify({ schema: this.schema, elements: this.states.filter(state => this.hasChanges(state)).map(state => state.contentReference) }));
  }

  persist(state) {
    if (this.hasChanges(state)) {
      localStorage.setItem(`cloudy:${JSON.stringify(state.contentReference)}`, JSON.stringify(state));
    } else {
      localStorage.removeItem(`cloudy:${JSON.stringify(state.contentReference)}`);
    }
    this.updateIndex();

    this.triggerAnyStateChange();
    this.triggerStateChange(state.contentReference);
  }

  unpersist(contentReference) {
    localStorage.removeItem(`cloudy:${JSON.stringify(contentReference)}`);
    this.updateIndex();

    this.triggerAnyStateChange();
    this.triggerStateChange(contentReference);
  }

  _onAnyStateChangeCallbacks = [];

  onAnyStateChange(callback) {
    this._onAnyStateChangeCallbacks.push(callback);
  }

  offAnyStateChange(callback) {
    this._onAnyStateChangeCallbacks.splice(this._onAnyStateChangeCallbacks.indexOf(callback), 1);
  }

  triggerAnyStateChange() {
    this._onAnyStateChangeCallbacks.forEach(callback => callback());
  }

  _onStateChangeCallbacks = {};

  onStateChange(contentReference, callback) {
    const key = JSON.stringify(contentReference);

    if (!this._onStateChangeCallbacks[key]) {
      this._onStateChangeCallbacks[key] = [];
    }

    this._onStateChangeCallbacks[key].push(callback);
  }

  offStateChange(contentReference, callback) {
    const key = JSON.stringify(contentReference);

    if (!this._onStateChangeCallbacks[key]) {
      this._onStateChangeCallbacks[key] = [];
    }

    this._onStateChangeCallbacks[key].splice(this._onStateChangeCallbacks[key].indexOf(callback), 1);
  }

  triggerStateChange(contentReference) {
    const key = JSON.stringify(contentReference);

    if (!this._onStateChangeCallbacks[key]) {
      this._onStateChangeCallbacks[key] = [];
    }

    this._onStateChangeCallbacks[key].forEach(callback => callback());
  }
}

export default new StateManager();