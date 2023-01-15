import arrayEquals from "../../util/array-equals";
import getReferenceValue from "../../util/get-reference-value";

class EmbeddedBlockChangeHandler {
  initState(state) {
    state.embeddedBlockChanges = [];
  }
  nullState(state) {
    state.embeddedBlockChanges = null;
  }
  discardChanges(state) {
    state.embeddedBlockChanges.splice(0, state.embeddedBlockChanges.length);
  }
  hasChanges(state, path = null) {
    if (path) {
      return state.embeddedBlockChanges?.find(c => arrayEquals(c.path, path))
    }

    return state.embeddedBlockChanges?.length;
  }
  addSavePayload(state, payload) {
    payload.embeddedBlockChanges = state.embeddedBlockChanges;

    return payload;
  }
  setType(stateManager, contentReference, path, type) {
    const state = stateManager.getState(contentReference);

    let change = state.embeddedBlockChanges.find(f => arrayEquals(path, f.path));

    if (!change) {
      change = { path };
      state.embeddedBlockChanges.push(change);
    }

    change.Type = type;

    const referenceValue = getReferenceValue(state, path);
    const referenceValueType = referenceValue ? referenceValue.Type : null;

    if ((type === '' && (referenceValueType === null || referenceValueType === undefined)) || referenceValueType == type) {
      state.embeddedBlockChanges.splice(state.embeddedBlockChanges.indexOf(change), 1); // remove changes that didn't change anything
    }

    stateManager.persist(state);

    stateManager.triggerAnyStateChange();
    stateManager.triggerStateChange(contentReference);
  }
  getIntermediateType(state, path) {
    const change = state.embeddedBlockChanges.find(c => arrayEquals(c.path, path));

    if (change) {
      return change.Type;
    }

    const referenceValue = getReferenceValue(state, path);
    return referenceValue ? referenceValue.Type : null;
  }
}

export default new EmbeddedBlockChangeHandler();