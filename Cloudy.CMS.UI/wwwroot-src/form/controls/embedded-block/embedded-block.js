import { html, useContext } from '../../../preact-htm/standalone.module.js';
import stateManager from '../../../data/state-manager.js';
import EntityContext from '../../entity-context.js';
import EmbeddedBlockFields from './embedded-block-fields.js';
import Dropdown from '../../../components/dropdown.js';
import closeDropdown from '../../../components/close-dropdown.js';

const Control = ({ name, path, settings: { types } }) => {
  const { contentReference, state } = useContext(EntityContext);

  return html`<div>
      <${Dropdown} text="Add">
        ${types.map(type => html`<a class="dropdown-item" onClick=${ event => { closeDropdown(event.target); } }>${type}</a>`)}
      <//>
    </div>`;
}
//<${EmbeddedBlockFields} type="HeroBlock"/>
export default Control;