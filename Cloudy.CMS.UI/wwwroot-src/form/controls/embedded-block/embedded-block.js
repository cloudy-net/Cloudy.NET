
import EmbeddedBlockFields from './embedded-block-fields.js';
import Dropdown from '../../../components/dropdown.js';
import closeDropdown from '../../../components/close-dropdown.js';

const Control = ({ name, label, path, settings: { types }, dependencies }) => {
  const { entityReference, state } = dependencies.useContext(dependencies.EntityContext);

  const type = dependencies.blockTypeChangeHandler.getIntermediateType(state, path);

  if (type) {
    const dropdown = dependencies.html`<${Dropdown} text="More">
      <a class="dropdown-item" onClick=${event => { dependencies.blockTypeChangeHandler.setType(entityReference, path, null); closeDropdown(event.target); }}>Remove</a>
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
        <${Dropdown} text="Add">
          ${types.map(type => dependencies.html`<a class="dropdown-item" onClick=${event => { dependencies.blockTypeChangeHandler.setType(entityReference, path, type); closeDropdown(event.target); }}>${type}</a>`)}
        <//>
      <//>
    `;
}

export default Control;