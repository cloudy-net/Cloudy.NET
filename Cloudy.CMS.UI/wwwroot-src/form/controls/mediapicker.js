import { useState, html, useContext } from '../../preact-htm/standalone.module.js';
import closeDropdown from "../../components/close-dropdown.js";
import Dropdown from "../../components/dropdown.js";
import MediaPickerMenu from "../../media-picker/media-picker-menu.js";
import stateManager from '../../data/state-manager.js';
import getIntermediateSimpleValue from '../../util/get-intermediate-simple-value.js';
import EntityContext from '../entity-context.js';

export default ({ name, path, provider }) => {
  const { contentReference, state } = useContext(EntityContext);
  const [value, setValue] = useState(getIntermediateSimpleValue(state, path));

  const copy = async () => {
    await navigator.clipboard.writeText(value);
  };

  const paste = async () => {
    const text = await navigator.clipboard.readText();

    setValue(text);
  };

  const onchange = newValue => {
    setValue(newValue != value ? newValue : null);
    stateManager.registerSimpleChange(contentReference, path, newValue);
  }

  return html`
    <input type="hidden" class="form-control" name=${name} value=${value} />

    ${value && html`<div class="mb-2">
      <img class="media-picker-preview-image" src=${value} />
    </div>`}

    <${Dropdown} text="Add">
      <${MediaPickerMenu} provider=${provider} value=${value} onSelect=${onchange} />
    <//>

    <${Dropdown} text="Other" className="ms-2">
      <a class="dropdown-item" onClick=${ event => { copy(); closeDropdown(event.target); } }>Copy</a>
      <a class="dropdown-item" onClick=${ event => { paste(); closeDropdown(event.target); } }>Paste</a>
      <a class="dropdown-item" onClick=${ event => { setValue(''); closeDropdown(event.target); } }>Clear</a>
    <//>
  `;
};