import changeManager from "../change-manager.js";
import stateManager from "../state-manager.js";
import statePersister from "../state-persister.js";

const UNCHANGED = {};

class EmbeddedBlockListHandler {
  add(entityReference, path, type) {
    const state = stateManager.getState(entityReference);
    const change = changeManager.getOrCreateLatestChange(state, 'embeddedblocklist.add', path);

    change.date = Date.now();
    change.type = type;

    state.changes = changeManager.getChanges(state);

    statePersister.persist(state);
  }
}

export default new EmbeddedBlockListHandler();