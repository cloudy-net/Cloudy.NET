
const Control = ({ name, path, dependencies }) => {
  const { entityReference, state } = dependencies.useContext(dependencies.EntityContext);

  const onchange = event => {
    dependencies.simpleChangeHandler.setValue(entityReference, path, event.target.value)
  };
  return dependencies.html`<div>
      <input
        type="number"
        step="any"
        pattern="[0-9]+(.[0-9]+?)?"
        class="form-control"
        id=${dependencies.componentContextProvider.getIdentifier(path)}
        value=${dependencies.simpleChangeHandler.getIntermediateValue(state, path)}
        onInput=${onchange}
      />
    </div>`;
}

export default Control;