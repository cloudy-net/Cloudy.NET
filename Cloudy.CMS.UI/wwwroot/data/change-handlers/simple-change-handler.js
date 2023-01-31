import stateManager from "../state-manager.js";

const UNCHANGED = {};

class SimpleChangeHandler {
  setValue(entityReference, path, value) {
    const state = stateManager.getState(entityReference);
    const change = stateManager.getOrCreateLatestChange(state, 'simple', path);

    change.date = Date.now();
    change.value = value;

    if (change.value == stateManager.getSourceValue(state, path) && state.changes.filter(change => change.path == path).length == 1) {
      state.changes.splice(state.changes.indexOf(change), 1);
    }

    stateManager.persist(state);
  }
  getIntermediateValue(state, path) {
    let value = UNCHANGED;

    for (var change of state.changes) {
      if (change['$type'] == 'simple' && path == change.path) {
        value = change.value;
        continue;
      }
      if (change['$type'] == 'blocktype' && path.indexOf(`${change.path}.`) == 0) {
        value = change.value;
        continue;
      }
    }

    if (value == UNCHANGED) {
      return stateManager.getSourceValue(state, path);
    }

    return value;
  }
}

export default new SimpleChangeHandler();