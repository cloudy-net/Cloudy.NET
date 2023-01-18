import arrayEquals from "../../util/array-equals.js";
import getReferenceValue from "../../util/get-reference-value.js";

class EmbeddedBlockChangeHandler {
  setType(stateManager, contentReference, path, type) {
    const state = stateManager.getState(contentReference);

    const referenceValue = getReferenceValue(state, path);
    const referenceValueType = referenceValue ? referenceValue.Type : null;

    if ((type === '' && (referenceValueType === null || referenceValueType === undefined)) || referenceValueType == type) {
      return;
    }

    let change = state.changes.find(c => c['$type'] == 'embeddedblock' && arrayEquals(path, c.path));

    if (!change) {
      change = { '$type': 'embeddedblock', 'date': Date.now(), path };
      state.changes.push(change);
    }

    change.Type = type;

    stateManager.persist(state);
  }
  getIntermediateType(state, path) {
    const change = state.changes.find(c => c['$type'] == 'embeddedblock' && arrayEquals(c.path, path));

    if (change) {
      return change.Type;
    }

    const referenceValue = getReferenceValue(state, path);
    return referenceValue ? referenceValue.Type : null;
  }
}

export default new EmbeddedBlockChangeHandler();