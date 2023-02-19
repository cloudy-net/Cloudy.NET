
const Control = ({ name, path, dependencies }) => {
  const { entityReference, state } = dependencies.useContext(dependencies.EntityContext);

  const onchange = event => {
    dependencies.simpleChangeHandler.setValue(entityReference, path, event.target.value)
  };
  return dependencies.html`<div>
      <textarea
        type="text"
        class="form-control"
        id=${`field-${name}`}
        name=${name}
        value=${dependencies.simpleChangeHandler.getIntermediateValue(state, path)}
        onInput=${onchange}
      />
    </div>`;
}

export default Control;