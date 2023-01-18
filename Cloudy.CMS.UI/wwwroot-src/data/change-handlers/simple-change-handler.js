import arrayEquals from "../../util/array-equals.js";
import arrayStartsWith from "../../util/array-starts-with.js";
import getReferenceValue from "../../util/get-reference-value.js";

const UNCHANGED = {};

class SimpleChangeHandler {
  registerChange(stateManager, contentReference, path, value) {
    const state = stateManager.getState(contentReference);
    const change = stateManager.getOrCreateLatestChange(state, 'simple', path);

    change.date = Date.now();
    change.value = value;

    stateManager.persist(state);
  }
  getIntermediateValue(state, path) {
    let value = UNCHANGED;

    for (var change of state.changes) {
      if (change['$type'] == 'simple' && arrayEquals(path, change.path)) {
        value = change.value;
        continue;
      }
      if (change['$type'] == 'embeddedblock' && arrayStartsWith(path, change.path)) {
        value = change.value;
        continue;
      }
    }

    if (value == UNCHANGED) {
      return getReferenceValue(state, path);
    }

    return value;
  }
}

export default new SimpleChangeHandler();