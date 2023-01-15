import arrayEquals from "../../util/array-equals";
import getReferenceValue from "../../util/get-reference-value";

class SimpleChangeHandler {
  registerChange(stateManager, contentReference, path, value) {
    const state = stateManager.getState(contentReference);

    let change = state.changes.find(c => c['$type'] == 'simple' && arrayEquals(path, c.path));

    if (!change) {
      change = { '$type': 'simple', 'date': Date.now(), path };
      state.changes.push(change);
    }

    change.value = value;

    const initialValue = getReferenceValue(state, path);

    if ((value === '' && (initialValue === null || initialValue === undefined)) || initialValue == value) {
      state.changes.splice(state.changes.indexOf(change), 1); // remove changes that didn't change anything
    }

    stateManager.persist(state);

    stateManager.triggerAnyStateChange();
    stateManager.triggerStateChange(contentReference);
  }
  getIntermediateValue(state, path) {
    let value = getReferenceValue(state, path);

    const change = state.changes.find(c => c['$type'] == 'simple' && arrayEquals(c.path, path));

    if (change) {
      value = change.value;
    }

    return value;
  }
}

export default new SimpleChangeHandler();