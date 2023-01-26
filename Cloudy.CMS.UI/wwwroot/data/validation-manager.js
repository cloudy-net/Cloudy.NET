import stateManager from "./state-manager.js";
import simpleChangeHandler from "./change-handlers/simple-change-handler.js";

const ValidationManager = {
  isValid: (validatorName, path, entityReference) => {

    let inValid = false;

    const state = stateManager.getState(entityReference);
    const value = simpleChangeHandler.getIntermediateValue(state, path);

    if (validatorName == 'required' && !value) inValid = true;
    // do other validation

    if (inValid && !state.invalidFields.includes(path)) {
      state.invalidFields.push(path);
      stateManager.persist(state)
    } else if (!inValid && state.invalidFields.includes(path)) {
      state.invalidFields = state.invalidFields.filter(i => i !== path);
      stateManager.persist(state)
    }

    return !inValid;
  },
}

export default ValidationManager;