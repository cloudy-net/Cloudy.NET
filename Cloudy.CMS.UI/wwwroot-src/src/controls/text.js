
const Control = ({ name, path, validators, dependencies }) => {
  const { entityReference, state } = dependencies.useContext(dependencies.EntityContext);

  return dependencies.html`
      <input
        type="text"
        class=${`form-control ${ dependencies.ValidationManager.getValidationClass(state.validationResults, path) } `}
        id=${dependencies.componentContextProvider.getIdentifier(path)}
        value=${dependencies.simpleChangeHandler.getIntermediateValue(state, path)}
        onInput=${(e) => dependencies.simpleChangeHandler.setValue(entityReference, path, e.target.value, validators)}
      />`;
}

export default Control;