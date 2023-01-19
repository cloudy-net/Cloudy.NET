import arrayEquals from "../../util/array-equals.js";
import arrayStartsWith from "../../util/array-starts-with.js";
import getReferenceValue from "../../util/get-reference-value.js";

const UNCHANGED = {};

class EmbeddedBlockChangeHandler {
  setType(stateManager, contentReference, path, type) {
    const state = stateManager.getState(contentReference);
    const change = stateManager.getOrCreateLatestChange(state, 'embeddedblock', path);

    change.date = Date.now();
    change.type = type;

    stateManager.persist(state);
  }
  getIntermediateType(state, path) {
    let type = UNCHANGED;

    for (var change of state.changes) {
      if (change['$type'] == 'embeddedblock' && arrayEquals(path, change.path)) {
        type = change.type;
        continue;
      }
      if (change['$type'] == 'embeddedblock' && arrayStartsWith(path, change.path)) {
        type = null;
        continue;
      }
    }

    if (type == UNCHANGED) {
      const referenceValue = getReferenceValue(state, path);
      return referenceValue ? referenceValue.Type : null;
    }

    return type;
  }
}

export default new EmbeddedBlockChangeHandler();