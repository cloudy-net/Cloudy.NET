import stateManager from "./state-manager";
import simpleChangeHandler from "./change-handlers/simple-change-handler";
import statePersister from "./state-persister";

class ValidationManager {
  validateAll(fields, entityReference)  {

    const state = stateManager.getState(entityReference);
    let validationResults = state.validationResults.slice();

    fields.forEach(field => {
      const value = simpleChangeHandler.getIntermediateValue(state, field.name);
      validationResults = this.getValidationResults(field.validators, field.name, validationResults, value);
    })

    state.validationResults = validationResults;
    statePersister.persist(state);

    return validationResults.every(vr => vr.isValid);
  }

  getValidationResults(validators, path, validationResults, value) {

    validators && Object.keys(validators).map(validatorName => {

      const validator = validators[validatorName];

      let inValid = false;
      if (validatorName == 'required' && !value) inValid = true;
      if (validatorName == 'maxLength' && !!value && value.length > validator.maxLength) inValid = true;

      //remove any existing VR for this path and validator
      validationResults = validationResults.filter(vr => vr.path != path || vr.validatorName != validatorName);

      //add a new VR
      validationResults.push({ path, validatorName, isValid: !inValid });
    });

    return validationResults;
  }

  isInvalidForPath(validationResults, path) {
    return validationResults.some(vr => !vr.isValid && vr.path == path);
  }

  isInvalidForPathAndValidator(validationResults, path, validatorName) {
    return validationResults.some(vr => !vr.isValid && vr.path == path && vr.validatorName == validatorName);
  }

  anyIsInvalid(validationResults) { return validationResults.some(vr => !vr.isValid) }

  getValidationClass(validationResults, path) { return this.isInvalidForPath(validationResults || [], path) ? 'is-invalid' : '' }
}

export default new ValidationManager();