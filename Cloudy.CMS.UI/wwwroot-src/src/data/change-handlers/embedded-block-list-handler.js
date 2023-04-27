import generateRandomString from "../../util/generate-random-string.js";
import changeManager from "../change-manager.js";
import stateManager from "../state-manager.js";
import statePersister from "../state-persister.js";

class EmbeddedBlockListHandler {
  add(entityReference, path, type) {
    const state = stateManager.getState(entityReference);
    const change = changeManager.getOrCreateLatestChange(state, 'embeddedblocklist.add', path);

    change.key = generateRandomString();
    change.date = Date.now();
    change.type = type;

    state.changes = changeManager.getChanges(state);

    statePersister.persist(state);

    return change;
  }
  remove(entityReference, path, key, type) {
    const state = stateManager.getState(entityReference);
    const change = changeManager.getOrCreateLatestChange(state, 'embeddedblocklist.remove', path);

    change.key = key;
    change.date = Date.now();
    change.type = type;

    state.changes = changeManager.getChanges(state);

    statePersister.persist(state);

    return change;
  }
  getIntermediateValue(state, path) {
    let value = (changeManager.getSourceValue(state.source.value, path) || []).map(({ Type }, key) => ({ key: `${key}`, type: Type }));

    for (var change of state.history) {
      if (change.$type == 'embeddedblocklist.add' && path == change.path) {
        value.push({ key: change.key, type: change.type });
        continue;
      }
      if (change.$type == 'embeddedblocklist.remove' && path == change.path) {
        value.splice(value.findIndex(element => element.key == change.key), 1);
        continue;
      }
      if (change.$type == 'blocktype' && path.indexOf(`${change.path}.`) == 0) {
        value = null;
        continue;
      }
    }

    return value;
  }
}

export default new EmbeddedBlockListHandler();