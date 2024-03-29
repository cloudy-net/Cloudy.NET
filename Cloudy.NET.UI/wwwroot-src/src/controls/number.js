
const Control = ({ name, path, dependencies }) => {
  const { entityReference, state } = dependencies.useContext(dependencies.EntityContext);

  const onchange = event => {
    dependencies.simpleChangeHandler.setValue(entityReference.value, path, event.target.value)
  };
  return dependencies.html`<div>
      <input
        type="number"
        pattern="[0-9]+"
        class="form-control"
        id=${dependencies.componentContextProvider.getIdentifier(path)}
        value=${dependencies.simpleChangeHandler.getIntermediateValue(state.value, path)}
        onInput=${onchange}
      />
    </div>`;
}

export default Control;