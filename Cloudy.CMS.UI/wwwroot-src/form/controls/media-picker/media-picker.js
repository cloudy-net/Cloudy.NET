import { useState, html, useContext } from '../../../preact-htm/standalone.module.js';
import closeDropdown from "../../../components/close-dropdown.js";
import Dropdown from "../../../components/dropdown.js";
import MediaPickerMenu from "./media-picker-menu.js";
import stateManager from '../../../data/state-manager.js';
import EntityContext from '../../entity-context.js';
import simpleChangeHandler from '../../../data/change-handlers/simple-change-handler.js';

export default ({ name, path, provider }) => {
  const { contentReference, state } = useContext(EntityContext);
  const [value, setValue] = useState(simpleChangeHandler.getIntermediateValue(state, path));

  const copy = async () => {
    await navigator.clipboard.writeText(value);
  };

  const paste = async () => {
    const text = await navigator.clipboard.readText();

    setValue(text);
  };

  const onchange = newValue => {
    setValue(newValue != value ? newValue : null);
    simpleChangeHandler.registerChange(stateManager, contentReference, path, newValue);
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
      <a class="dropdown-item" onClick=${ event => { setValue(''); simpleChangeHandler.registerChange(stateManager, contentReference, path, ''); closeDropdown(event.target); } }>Clear</a>
    <//>
  `;
};