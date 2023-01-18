import arrayEquals from "../../util/array-equals.js";
import getReferenceValue from "../../util/get-reference-value.js";

class SimpleChangeHandler {
  registerChange(stateManager, contentReference, path, value) {
    const state = stateManager.getState(contentReference);
    const change = stateManager.getOrCreateLatestChange(state, 'simple', path);

    change.date = Date.now();
    change.value = value;

    stateManager.persist(state);
  }
  getIntermediateValue(state, path) {
    const changes = state.changes.filter(c => c['$type'] == 'simple' && arrayEquals(c.path, path));

    if(changes.length){
      return changes[changes.length - 1].value;
    }

    return getReferenceValue(state, path);
  }
}

export default new SimpleChangeHandler();