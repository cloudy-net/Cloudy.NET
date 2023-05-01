import changeManager from "../change-manager";
import EntityReference from "../entity-reference";
import State from "../state";
import stateManager from "../state-manager";
import statePersister from "../state-persister";
import ValidationManager from "../validation-manager";

const UNCHANGED = {};

class SimpleChangeHandler {
  setValue(entityReference: EntityReference, path: string, value: any, validators?: any[]) {
    const state = stateManager.getState(entityReference)!;
    const change = changeManager.getOrCreateLatestChange(state, 'simple', path);

    change.date = Date.now();
    change.value = value;

    if (change.value == changeManager.getSourceValue(state.source!.value, path) && state.history.filter(change => change.path == path).length == 1) {
      state.history.splice(state.history.indexOf(change), 1);
    }

    if (validators && Object.keys(validators).length) {
      state.validationResults = ValidationManager.getValidationResults(validators, path, state.validationResults.slice(), value);
    }

    state.changes = changeManager.getChanges(state);

    statePersister.persist(state);
  }
  getIntermediateValue(state: State, path: string) {
    let value = UNCHANGED;

    for (var change of state.history) {
      if (change.$type == 'simple' && path == change.path) {
        value = change.value;
        continue;
      }
      if (change.$type == 'blocktype' && path.indexOf(`${change.path}.`) == 0) {
        value = change.value;
        continue;
      }
    }

    if (value == UNCHANGED) {
      return changeManager.getSourceValue(state.source!.value, path);
    }

    return value;
  }
}

export default new SimpleChangeHandler();