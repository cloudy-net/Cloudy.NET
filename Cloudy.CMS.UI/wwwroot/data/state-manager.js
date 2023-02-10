import urlFetcher from "../util/url-fetcher.js";
import notificationManager from "../notification/notification-manager.js";
import ContentNotFound from "./content-not-found.js";
import statePersister from "./state-persister.js";
import changeManager from "./change-manager.js";
import conflictManager from "./conflict-manager.js";

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

class StateManager {
  states = statePersister.loadStates();

  getAll() {
    return this.states.filter(state => changeManager.hasChanges(state));
  }

  createStateForNewContent(entityType) {
    const entityReference = { newContentKey: generateRandomString(), keyValues: null, entityType };

    const state = {
      new: true,
      validationResults: [],
      entityReference,
      source: {
        value: {},
        properties: {},
        date: new Date(),
      },
      history: [],
      conflicts: [],
      changes: [],
    };
    this.states.push(state);
    statePersister.persist(state);

    return state;
  };

  createOrUpdateStateForExistingContent(entityReference, nameHint) {
    const existingState = this.getState(entityReference);
    if (existingState) {
      this.reloadContentForState(entityReference);
      return existingState;
    }

    const state = {
      new: false,
      validationResults: [],
      entityReference,
      loading: true,
      nameHint,
      source: null,
      history: null,
      conflicts: null,
      changes: null,
    };
    this.states.push(state);
    statePersister.persist(state);

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

    const changes = changeManager.getChanges(state);

    if (JSON.stringify(state.source.value) == JSON.stringify(entity)) {
      state = {
        ...state,
        loadingNewSource: false,
        conflicts: [],
        changes,
      };
    } else {
      if (!changeManager.hasChanges(state)) {
        state = {
          ...state,
          loadingNewSource: false,
          source: {
            value: entity,
            properties: response.type.properties,
            date: new Date(),
          },
          conflicts: [],
          changes,
        };
      } else {
        state = {
          ...state,
          loadingNewSource: false,
          newSource: {
            value: entity,
            date: new Date(),
            properties: response.type.properties,
          },
          conflicts: conflictManager.getSourceConflicts(state, changes),
          changes,
        };
      }

      this.replace(state);
    }
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
        properties: response.type.properties,
        date: new Date(),
      },
      history: [],
      conflicts: [],
    };

    state.changes = changeManager.getChanges(state);
    
    this.replace(state);
  }

  async save(entityReferences) {
    await Promise.allSettled(entityReferences.map(entityReference => this.reloadContentForState(entityReference)));
    
    const states = entityReferences.map(c => this.getState(c));

    if(states.filter(state => state.conflicts.length).length){
      return;
    }

    const response = await urlFetcher.fetch("/Admin/api/form/entity/save", {
      credentials: "include",
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        entities: states.map(state => ({
          reference: state.entityReference,
          changes: state.history.map(change => {
            change = {
              ...change,
              date: new Date(change.date),
              path: change.path.split('.'),
            };

            if (change.$type == 'simple') {
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
    statePersister.persist(state);
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
}

export default new StateManager();
