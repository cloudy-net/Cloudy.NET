import { html, useContext } from '../../../preact-htm/standalone.module.js';
import EntityContext from '../../entity-context.js';
import EmbeddedBlockFields from './embedded-block-fields.js';
import Dropdown from '../../../components/dropdown.js';
import closeDropdown from '../../../components/close-dropdown.js';
import blockTypeChangeHandler from '../../../data/change-handlers/block-type-change-handler.js';

const Control = ({ name, label, path, settings: { types } }) => {
  const { entityReference, state } = useContext(EntityContext);

  const type = blockTypeChangeHandler.getIntermediateType(state, path);

  if (type) {
    const dropdown = html`<${Dropdown} text="More">
      <a class="dropdown-item" onClick=${event => { blockTypeChangeHandler.setType(entityReference, path, null); closeDropdown(event.target); }}>Remove</a>
    <//>`;
    return html`<div class="mb-3">
      <label for=${name} class="form-label">${label} ${state.changes.find(change => change.path == path) ? '*' : null} ${dropdown}</label>
      <fieldset class="m-2">
        <${EmbeddedBlockFields} ...${{type, path}}/>
      <//>
    <//>`;
  }

  return html`<div class="mb-3">
        <label for=${name} class="form-label">${label} ${state.changes.find(change => change.path == path) ? '*' : null}</label>
        <${Dropdown} text="Add">
          ${types.map(type => html`<a class="dropdown-item" onClick=${event => { blockTypeChangeHandler.setType(entityReference, path, type); closeDropdown(event.target); }}>${type}</a>`)}
        <//>
      <//>
    `;
}

export default Control;