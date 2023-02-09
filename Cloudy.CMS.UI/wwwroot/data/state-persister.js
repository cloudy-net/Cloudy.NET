import changeManager from "./change-manager.js";
import stateEvents from "./state-events.js";
import stateManager from "./state-manager.js";

class StatePersister {
  indexStorageKey = "cloudy:statesIndex";
  schema = "1.12";

  loadStates() {
    let index = JSON.parse(localStorage.getItem(this.indexStorageKey) || JSON.stringify({ schema: this.schema, elements: [] }));

    if (index.schema != this.schema) {
      if (confirm(`Warning: The state schema has changed (new version: ${this.schema}, old version: ${index.schema}).\n\nThis means the format of local state has changed, and your local change.s are no longer understood by the Admin UI.\n\nYou are required to clear your local change.s to avoid any strange bugs.\n\nPress OK to continue, or cancel to do the necessary schema change.s manually to your localStorage (not supported officially).`)) {
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

  updateIndex() {
    localStorage.setItem(this.indexStorageKey, JSON.stringify({ schema: this.schema, elements: stateManager.states.filter(state => changeManager.hasChanges(state)).map(state => state.entityReference) }));
  }

  persist(state) {
    if (changeManager.hasChanges(state)) {
      localStorage.setItem(`cloudy:${JSON.stringify(state.entityReference)}`, JSON.stringify(state));
    } else {
      localStorage.removeItem(`cloudy:${JSON.stringify(state.entityReference)}`);
    }
    this.updateIndex();

    stateEvents.triggerAnyStateChange();
    stateEvents.triggerStateChange(state.entityReference);
  }

  unpersist(entityReference) {
    localStorage.removeItem(`cloudy:${JSON.stringify(entityReference)}`);
    this.updateIndex();

    stateEvents.triggerAnyStateChange();
    stateEvents.triggerStateChange(entityReference);
  }
}

export default new StatePersister();