import urlFetcher from "../util/url-fetcher.js";
import notificationManager from "../notification/notification-manager.js";
import EntityNotFound from "./entity-not-found.js";
import statePersister from "./state-persister.js";
import changeManager from "./change-manager.js";
import conflictManager from "./conflict-manager.js";
import stateEvents from "./state-events.js";
import generateRandomString from "../util/generate-random-string.js";
import arrayEquals from "../util/array-equals.js";

const entityReferenceEquals = (a, b) => arrayEquals(a.keyValues, b.keyValues) && a.newEntityKey == b.newEntityKey && a.entityType == b.entityType;

class StateManager {
  states = statePersister.loadStates();

  getAll() {
    return this.states.filter(state => state.changes.length);
  }

  createStateForNewEntity(entityType) {
    const entityReference = { newEntityKey: generateRandomString(), keyValues: null, entityType };

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

  createOrUpdateStateForExistingEntity(entityReference, nameHint) {
    const existingState = this.getState(entityReference);
    if (existingState) {
      this.reloadEntityForState(entityReference);
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

    this.loadEntityForState(entityReference);

    return state;
  };

  async reloadEntityForState(entityReference) {
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
      `Could not get entity ${JSON.stringify(entityReference.keyValues)} (${entityReference.entityType})`,
      {
        410: () => new EntityNotFound(entityReference)
      }
    );

    const entity = response.entity.Value;

    state = this.getState(entityReference);

    if (JSON.stringify(state.source.value) == JSON.stringify(entity)) {
      state = {
        ...state,
        loadingNewSource: false,
        conflicts: [],
      };
    } else {
      if (!state.changes.length) {
        state = {
          ...state,
          loadingNewSource: false,
          source: {
            value: entity,
            properties: response.type.properties,
            date: new Date(),
          },
          conflicts: [],
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
        };
        state.conflicts = conflictManager.getConflicts(state);
      }

      this.replace(state);
    }
  }

  async loadEntityForState(entityReference) {
    const response = await urlFetcher.fetch(
      `/Admin/api/form/entity/get`,
      {
        credentials: 'include',
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(entityReference)
      },
      `Could not get entity ${JSON.stringify(entityReference.keyValues)} (${entityReference.entityType})`,
      {
        410: () => new EntityNotFound(entityReference)
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

  async save(state) {
    if(!state.new) {
      await this.reloadEntityForState(state.entityReference);
    }

    if(state.conflicts.length){
      return;
    }

    const result = (await this.saveInternal([state.entityReference]))[0];

    if (result.success) {
      notificationManager.addNotification((item) => item.setText('Entity has been saved.'));

      this.remove(state.entityReference);

      const entityReference = result.entityReference;

      entityReference.keyValues = entityReference.keyValues.map(v => `${v}`);

      this.createOrUpdateStateForExistingEntity(entityReference);
      stateEvents.triggerEntityReferenceChange(entityReference);
      stateEvents.triggerStateChange(this.getState(entityReference));
    }

    return result;
  }

  async saveInternal(entityReferences) {
    const states = entityReferences.map(c => this.getState(c));

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

            return change;
          }),
        }))
      }),
    }, 'Could not save entity');

    return response.results;
  }

  replace(state) {
    this.states[this.states.findIndex(s => entityReferenceEquals(s.entityReference, state.entityReference))] = state;
    statePersister.persist(state);
  }

  remove(entityReference) {
    this.states.splice(this.states.findIndex(s => entityReferenceEquals(s.entityReference, entityReference)), 1);
    statePersister.unpersist(entityReference);

    return entityReference;
  };

  getState(entityReference) {
    return this.states.find(s => entityReferenceEquals(s.entityReference, entityReference));
  }
}

export default new StateManager();
