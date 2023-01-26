import urlFetcher from "../util/url-fetcher.js";
import notificationManager from "../notification/notification-manager.js";
import ContentNotFound from "./content-not-found.js";

const generateRandomString = () => (Math.random() * 0xFFFFFF << 0).toString(16).padStart(6, '0'); // https://stackoverflow.com/questions/5092808/how-do-i-randomly-generate-html-hex-color-codes-using-javascript
const arrayEquals = (a, b) => {
  if (a == null && b == null) {
    return true;
  }

  if (a == null) {
    return false;
  }

  if (b == null) {
    return false;
  }

  if (!Array.isArray(a) || !Array.isArray(b)) {
    return false;
  }

  if (a.length != b.length) {
    return false;
  }

  return a.every((ai, i) => ai === b[i]);
};
const entityReferenceEquals = (a, b) => arrayEquals(a.keyValues, b.keyValues) && a.newContentKey == b.newContentKey && a.entityType == b.entityType;

const FIVE_MINUTES = 5 * 60 * 1000;

class StateManager {
  indexStorageKey = "cloudy:statesIndex";
  schema = "1.10";
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

    for (let entityReference of index.elements) {
      result.push(JSON.parse(localStorage.getItem(`cloudy:${JSON.stringify(entityReference)}`), (key, value) => key == 'referenceDate' && value ? new Date(value) : value));
    }

    return result;
  }

  getAll() {
    return this.states.filter(state => this.hasChanges(state));
  }

  createStateForNewContent(entityType) {
    const entityReference = { newContentKey: generateRandomString(), keyValues: null, entityType };

    const state = {
      new: true,
      entityReference,
      source: {
        value: {},
        date: new Date(),
      },
      changes: [],
    };
    this.states.push(state);
    this.persist(state);

    return state;
  };

  createOrUpdateStateForExistingContent(entityReference, nameHint) {
    const existingState = this.getState(entityReference);
    if (existingState) {
      this.reloadContentForState(entityReference);
      return existingState;
    }

    const state = {
      entityReference,
      loading: true,
      nameHint,
      source: null,
      changes: null,
    };
    this.states.push(state);
    this.persist(state);

    this.loadContentForState(entityReference);

    return state;
  };

  async reloadContentForState(entityReference) {
    let state = this.getState(entityReference);

    state = {
      ...state,
      loadingNewSource: true,
      newSource: null,
    };
    this.replace(state);

    const response = await urlFetcher.fetch(
      `/Admin/api/form/entity/get`,
      {
        credentials: 'include',
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(entityReference)
      },
      `Could not get content ${JSON.stringify(entityReference.keyValues)} (${entityReference.entityType})`,
      {
        410: () => new ContentNotFound(entityReference)
      }
    );

    const entity = response.entity.Value;

    state = this.getState(entityReference);

    if (JSON.stringify(state.source.value) == JSON.stringify(entity)) {
      state = {
        ...state,
        loadingNewSource: false,
      };
    } else {
      if (!this.hasChanges(state)) {
        state = {
          ...state,
          loadingNewSource: false,
          source: {
            value: entity,
            date: new Date(),
          },
        };
      } else {
        state = {
          ...state,
          loadingNewSource: false,
          newSource: {
            value: entity,
            date: new Date(),
          },
        };
      }

      this.replace(state);
    }
  }

  discardnewSource(entityReference) {
    let state = this.getState(entityReference);

    state = {
      ...state,
      source: state.newSource,
      newSource: null,
    };
    this.replace(state);
  }

  async loadContentForState(entityReference) {
    const response = await urlFetcher.fetch(
      `/Admin/api/form/entity/get`,
      {
        credentials: 'include',
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(entityReference)
      },
      `Could not get content ${JSON.stringify(entityReference.keyValues)} (${entityReference.entityType})`,
      {
        410: () => new ContentNotFound(entityReference)
      }
    );

    const entity = response.entity.Value;

    let state = this.getState(entityReference);

    state = {
      ...state,
      loading: false,
      nameHint: null,
      source: {
        value: entity,
        date: new Date(),
      },
      changes: [],
    };

    this.replace(state);
  }

  getOrCreateLatestChange(state, type, path) {
    let change = null;

    for (let c of state.changes) {
      if (c['$type'] == type && path == c.path) {
        change = c;
        continue;
      }
      if (c['$type'] == 'blocktype' && path.indexOf(`${c.path}.`) == 0) {
        change = null;
        continue;
      }
      if (c['$type'] == 'simple' && c.path.indexOf(`${path}.`) == 0) {
        change = null;
        continue;
      }
    }

    if (!change || Date.now() - change.date > FIVE_MINUTES) {
      change = { '$type': type, id: generateRandomString(), 'date': Date.now(), path };
      state.changes.push(change);
    }

    return change;
  }

  async save(entityReferences) {
    const states = entityReferences.map(c => this.getState(c));
    const response = await urlFetcher.fetch("/Admin/api/form/entity/save", {
      credentials: "include",
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        entities: states.map(state => ({
          reference: state.entityReference,
          changes: state.changes.map(change => {
            change = {
              ...change,
              date: new Date(change.date),
              path: change.path.split('.'),
            };

            if (change['$type'] == 'simple') {
              change.value = JSON.stringify(change.value);
            }

            return change;
          }),
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

    for (let result of response.results.filter(r => r.success)) {
      this.loadContentForState(result.entityReference);
    }
  }

  replace(state) {
    this.states[this.states.findIndex(s => entityReferenceEquals(s.entityReference, state.entityReference))] = state;
    this.persist(state);
  }

  remove(entityReference) {
    this.states.splice(this.states.findIndex(s => entityReferenceEquals(s.entityReference, entityReference)), 1);
    this.unpersist(entityReference);

    return entityReference;
  };

  getState(entityReference) {
    if (entityReference.newContentKey && entityReference.keyValues) {
      entityReference = {
        ...entityReference,
        keyValues: null,
      };
    }
    return this.states.find(s => entityReferenceEquals(s.entityReference, entityReference));
  }

  discardChanges(entityReference, change) {
    const state = this.getState(entityReference);

    state.changes.splice(0, state.changes.length);

    this.persist(state);
  }

  hasChanges(state, path = null) {
    if (state.changes == null) {
      return false;
    }

    const changes = this.getMergedChanges(state, path);

    return changes.length;
  }

  getMergedChanges(state, path = null) {
    if (state.changes == null) {
      return [];
    }

    const changes = {};

    for (let change of state.changes.filter(change => path == null || change.path == path)) {
      if (change.$type == 'blocktype') {
        Object.keys(changes).filter(path => path.indexOf(`${change.path}.`) == 0).forEach(path => delete changes[path]);
      }

      changes[change.path] = change;
    }

    Object.values(changes).filter(change => change.$type == 'simple').filter(change => change.value == this.getSourceValue(state, change.path)).forEach(change => delete changes[change.path])

    return Object.values(changes);
  }

  getReferenceChanges(state) {
    return [];
  }

  getSourceValue(state, path) {
    let value = state.source.value;
    let pathSegments = path.split('.');

    while (pathSegments.length) {
      if (!value) {
        return null;
      }

      if (pathSegments.length > 1) {
        value = value[pathSegments[0]] ? value[pathSegments[0]].Value : null;
      } else {
        value = value[pathSegments[0]];
      }

      pathSegments = pathSegments.splice(1);
    }

    return value;
  }

  updateIndex() {
    localStorage.setItem(this.indexStorageKey, JSON.stringify({ schema: this.schema, elements: this.states.filter(state => this.hasChanges(state)).map(state => state.entityReference) }));
  }

  persist(state) {
    if (this.hasChanges(state)) {
      localStorage.setItem(`cloudy:${JSON.stringify(state.entityReference)}`, JSON.stringify(state));
    } else {
      localStorage.removeItem(`cloudy:${JSON.stringify(state.entityReference)}`);
    }
    this.updateIndex();

    this.triggerAnyStateChange();
    this.triggerStateChange(state.entityReference);
  }

  unpersist(entityReference) {
    localStorage.removeItem(`cloudy:${JSON.stringify(entityReference)}`);
    this.updateIndex();

    this.triggerAnyStateChange();
    this.triggerStateChange(entityReference);
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

  onStateChange(entityReference, callback) {
    const key = JSON.stringify(entityReference);

    if (!this._onStateChangeCallbacks[key]) {
      this._onStateChangeCallbacks[key] = [];
    }

    this._onStateChangeCallbacks[key].push(callback);
  }

  offStateChange(entityReference, callback) {
    const key = JSON.stringify(entityReference);

    if (!this._onStateChangeCallbacks[key]) {
      this._onStateChangeCallbacks[key] = [];
    }

    this._onStateChangeCallbacks[key].splice(this._onStateChangeCallbacks[key].indexOf(callback), 1);
  }

  triggerStateChange(entityReference) {
    const key = JSON.stringify(entityReference);

    if (!this._onStateChangeCallbacks[key]) {
      this._onStateChangeCallbacks[key] = [];
    }

    this._onStateChangeCallbacks[key].forEach(callback => callback());
  }
}

export default new StateManager();