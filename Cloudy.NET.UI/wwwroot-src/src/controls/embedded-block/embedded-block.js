
import EmbeddedBlockFields from './embedded-block-fields.js';

const Control = ({ name, label, path, settings: { types }, dependencies }) => {
  const { entityReference, state } = dependencies.useContext(dependencies.EntityContext);

  const type = dependencies.blockTypeChangeHandler.getIntermediateType(state, path);

  if (type) {
    const dropdown = dependencies.html`<${dependencies.Dropdown} text="More">
      <a class="dropdown-item" onClick=${event => { dependencies.blockTypeChangeHandler.setType(entityReference, path, null); dependencies.closeDropdown(event.target); }}>Remove</a>
    <//>`;
    return dependencies.html`<div class="mb-3">
      <label for=${name} class="form-label">${label} ${state.changes.find(change => change.path == path) ? '*' : null} ${dropdown}</label>
      <fieldset class="m-2">
        <${EmbeddedBlockFields} ...${{type, path, dependencies}}/>
      <//>
    <//>`;
  }

  return dependencies.html`<div class="mb-3">
        <label for=${name} class="form-label">${label} ${state.changes.find(change => change.path == path) ? '*' : null}</label>
        <${dependencies.Dropdown} text="Add">
          ${types.map(type => dependencies.html`<a class="dropdown-item" onClick=${event => { dependencies.blockTypeChangeHandler.setType(entityReference, path, type); dependencies.closeDropdown(event.target); }}>${type}</a>`)}
        <//>
      <//>
    `;
}

export default Control;