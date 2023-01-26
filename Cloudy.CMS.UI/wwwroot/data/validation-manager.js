
const ValidationManager = {
  getValidationResults: (validators, path, validationResults, value) => {

    validators && Object.keys(validators).map(validatorName => {

      let inValid = false;
      if (validatorName == 'required' && !value) inValid = true;

      //remove any existing VR for this path and validator
      validationResults = validationResults.filter(vr => vr.path != path || vr.validatorName != validatorName);

      //add a new VR
      validationResults.push({ path, validatorName, isValid: !inValid });
    });

    return validationResults;
  },
  isInvalidForPath: (validationResults, path) => {
    return validationResults.some(vr => !vr.isValid && vr.path == path);
  },
  isInvalidForPathAndValidator: (validationResults, path, validatorName) => {
    return validationResults.some(vr => !vr.isValid && vr.path == path && vr.validatorName == validatorName);
  },
  anyIsInvalid: (validationResults) => validationResults.some(vr => !vr.isValid),
}

export default ValidationManager;