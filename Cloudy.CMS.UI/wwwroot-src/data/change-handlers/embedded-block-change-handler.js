import arrayEquals from "../../util/array-equals";
import getReferenceValue from "../../util/get-reference-value";

class EmbeddedBlockChangeHandler {
  setType(stateManager, contentReference, path, type) {
    const state = stateManager.getState(contentReference);

    let change = state.changes.find(c => c['$type'] == 'embeddedblock' && arrayEquals(path, c.path));

    if (!change) {
      change = { '$type': 'embeddedblock', path };
      state.changes.push(change);
    }

    change.Type = type;

    const referenceValue = getReferenceValue(state, path);
    const referenceValueType = referenceValue ? referenceValue.Type : null;

    if ((type === '' && (referenceValueType === null || referenceValueType === undefined)) || referenceValueType == type) {
      state.changes.splice(state.changes.indexOf(change), 1); // remove changes that didn't change anything
    }

    stateManager.persist(state);

    stateManager.triggerAnyStateChange();
    stateManager.triggerStateChange(contentReference);
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