import arrayEquals from "../../util/array-equals.js";
import getReferenceValue from "../../util/get-reference-value.js";

class EmbeddedBlockChangeHandler {
  setType(stateManager, contentReference, path, type) {
    const state = stateManager.getState(contentReference);
    const change = stateManager.getOrCreateLatestChange(state, 'embeddedblock', path);

    change.date = Date.now();
    change.Type = type;

    stateManager.persist(state);
  }
  getIntermediateType(state, path) {
    const changes = state.changes.filter(c => c['$type'] == 'embeddedblock' && arrayEquals(c.path, path));

    if (changes.length) {
      return changes.Type;
    }

    const referenceValue = getReferenceValue(state, path);
    return referenceValue ? referenceValue.Type : null;
  }
}

export default new EmbeddedBlockChangeHandler();