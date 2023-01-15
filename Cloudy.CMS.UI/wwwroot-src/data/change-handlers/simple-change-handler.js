import arrayEquals from "../../util/array-equals";
import getReferenceValue from "../../util/get-reference-value";

class SimpleChangeHandler {
  initState(state) {
    state.simpleChanges = [];
  }
  nullState(state) {
    state.simpleChanges = null;
  }
  discardChanges(state) {
    state.simpleChanges.splice(0, state.simpleChanges.length);
  }
  hasChanges(state, path = null) {
    if (path) {
      return state.simpleChanges?.find(c => arrayEquals(c.path, path))
    }

    return state.simpleChanges?.length;
  }
  addSavePayload(state, payload){
    payload.simpleChanges = state.simpleChanges.map(simpleChange => ({
      ...simpleChange,
      value: JSON.stringify(simpleChange.value)
    }));

    return payload;
  }
  registerChange(stateManager, contentReference, path, value) {
    const state = stateManager.getState(contentReference);

    let change = state.simpleChanges.find(f => arrayEquals(path, f.path));

    if (!change) {
      change = { path };
      state.simpleChanges.push(change);
    }

    change.value = value;

    const initialValue = getReferenceValue(state, path);

    if ((value === '' && (initialValue === null || initialValue === undefined)) || initialValue == value) {
      state.simpleChanges.splice(state.simpleChanges.indexOf(change), 1); // remove changes that didn't change anything
    }

    stateManager.persist(state);

    stateManager.triggerAnyStateChange();
    stateManager.triggerStateChange(contentReference);
  }
  getIntermediateValue(state, path) {
    let value = getReferenceValue(state, path);

    const change = state.simpleChanges.find(c => arrayEquals(c.path, path));

    if (change) {
      value = change.value;
    }

    return value;
  }
}

export default new SimpleChangeHandler();