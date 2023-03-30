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
  getIntermediateValue(state, path) {
    let value = changeManager.getSourceValue(state.source.value, path).map((value, key) => `${key}`) || [];

    for (var change of state.history) {
      if (change.$type == 'embeddedblocklist.add' && path == change.path) {
        value.push(change.key);
        continue;
      }
      // if (change.$type == 'blocktype' && path.indexOf(`${change.path}.`) == 0) {
      //   value = change.value;
      //   continue;
      // }
    }

    return value;
  }
}

export default new EmbeddedBlockListHandler();