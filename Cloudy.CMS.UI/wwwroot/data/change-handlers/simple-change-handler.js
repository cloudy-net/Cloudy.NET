import stateManager from "../state-manager.js";

const UNCHANGED = {};

class SimpleChangeHandler {
  setValue(contentReference, path, value) {
    const state = stateManager.getState(contentReference);
    const change = stateManager.getOrCreateLatestChange(state, 'simple', path);

    change.date = Date.now();
    change.value = value;

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
      return stateManager.getReferenceValue(state, path);
    }

    return value;
  }
}

export default new SimpleChangeHandler();