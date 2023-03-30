import generateRandomString from "../../util/generate-random-string.js";
import changeManager from "../change-manager.js";
import stateManager from "../state-manager.js";
import statePersister from "../state-persister.js";

const UNCHANGED = {};

class EmbeddedBlockListHandler {
  add(entityReference, path, type) {
    const state = stateManager.getState(entityReference);
    const change = changeManager.getOrCreateLatestChange(state, 'embeddedblocklist.add', path);

    change.key = generateRandomString();
    change.date = Date.now();
    change.type = type;

    state.changes = changeManager.getChanges(state);

    statePersister.persist(state);
  }
  getIntermediateValue(state, path) {
    let value = UNCHANGED;

    for (var change of state.history) {
      if (change.$type == 'embeddedblocklist.add' && path == change.path) {
        if(value == UNCHANGED){
          value = [];
        }

        value.push(change.key);
        continue;
      }
      // if (change.$type == 'blocktype' && path.indexOf(`${change.path}.`) == 0) {
      //   value = change.value;
      //   continue;
      // }
    }

    if (value == UNCHANGED) {
      return changeManager.getSourceValue(state.source.value, path);
    }

    return value;
  }
}

export default new EmbeddedBlockListHandler();