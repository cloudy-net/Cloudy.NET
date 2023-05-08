
const Control = ({ name, path, dependencies }) => {
  const {
    useContext,
    EntityContext,
    simpleChangeHandler,
    componentContextProvider,
  } = dependencies;

  const { entityReference, state } = useContext(EntityContext);

  const onchange = event => {
    simpleChangeHandler.setValue(entityReference.value, path, event.target.value)
  };
  return dependencies.html`<div>
      <textarea
        type="text"
        class="form-control"
        id=${componentContextProvider.getIdentifier(path)}
        value=${simpleChangeHandler.getIntermediateValue(state.value, path)}
        onInput=${onchange}
      />
    </div>`;
}

export default Control;