import changeManager from "../change-manager";
import EntityReference from "../entity-reference";
import State from "../state";
import stateManager from "../state-manager";
import statePersister from "../state-persister";

const UNCHANGED = {};

class BlockTypeHandler {
  setType(entityReference: EntityReference, path: string, type: string | null) {
    const state = stateManager.getState(entityReference)!;
    const change = changeManager.getOrCreateLatestChange(state, 'blocktype', path);

    change.date = Date.now();
    change.type = type;

    state.changes = changeManager.getChanges(state);

    statePersister.persist(state);
  }
  getIntermediateType(state: State, path: string) {
    let type: any = UNCHANGED;

    for (var change of state.history) {
      if (change.$type == 'blocktype' && path == change.path) {
        type = change.type;
        continue;
      }
      if (change.$type == 'blocktype' && path.indexOf(`${change.path}.`) == 0) {
        type = null;
        continue;
      }
    }

    if (type == UNCHANGED) {
      const sourceValue = changeManager.getSourceValue(state.source!.value, path);
      return sourceValue ? sourceValue.Type : null;
    }

    return type;
  }
}

export default new BlockTypeHandler();