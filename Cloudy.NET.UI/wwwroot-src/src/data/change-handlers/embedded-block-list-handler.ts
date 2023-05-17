import generateRandomString from "../../util/generate-random-string";
import changeManager from "../change-manager";
import EntityReference from "../entity-reference";
import State from "../state";
import stateManager from "../state-manager";
import statePersister from "../state-persister";

class EmbeddedBlockListHandler {
  add(entityReference: EntityReference, path: string, type: string) {
    const state = stateManager.getState(entityReference)!;
    const change = changeManager.getOrCreateLatestChange(state, 'embeddedblocklist.add', path);

    change.key = generateRandomString();
    change.date = Date.now();
    change.type = type;

    state.changes = changeManager.getChanges(state);

    statePersister.persist(state);

    return change;
  }
  remove(entityReference: EntityReference, path: string, key: string, type: string | null | undefined) {
    const state = stateManager.getState(entityReference)!;
    const change = changeManager.getOrCreateLatestChange(state, 'embeddedblocklist.remove', path);

    change.key = key;
    change.date = Date.now();
    change.type = type;

    state.changes = changeManager.getChanges(state);

    statePersister.persist(state);

    return change;
  }
  getIntermediateValue(state: State, path: string) {
    let value = (changeManager.getSourceValue(state.source!.value, path) || []).map(({ Type }: { Type: string }, key: string) => ({ key: `${key}`, type: Type }));

    for (var change of state.history) {
      if (change.$type == 'embeddedblocklist.add' && path == change.path) {
        value.push({ key: change.key, type: change.type });
        continue;
      }
      if (change.$type == 'embeddedblocklist.remove' && path == change.path) {
        value.splice(value.findIndex((element: { key: string }) => element.key == change.key), 1);
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