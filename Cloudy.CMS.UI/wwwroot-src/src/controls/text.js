
const Control = ({ name, path, validators, dependencies }) => {
  const {
    html,
    useContext,
    EntityContext,
    ValidationManager,
    componentContextProvider,
    simpleChangeHandler,
  } = dependencies;

  const { entityReference, state } = useContext(EntityContext);

  return html`
      <input
        type="text"
        class=${`form-control ${ ValidationManager.getValidationClass(state.value.validationResults, path) } `}
        id=${componentContextProvider.getIdentifier(path)}
        value=${simpleChangeHandler.getIntermediateValue(state.value, path)}
        onInput=${(e) => simpleChangeHandler.setValue(entityReference.value, path, e.target.value, validators)}
      />`;
}

export default Control;