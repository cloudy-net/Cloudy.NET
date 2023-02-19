import closeDropdown from "../../../components/close-dropdown.js";
import Dropdown from "../../../components/dropdown.js";
import MediaPickerMenu from "./media-picker-menu.js";

export default ({ name, path, provider, dependencies }) => {
  const { entityReference, state } = dependencies.useContext(dependencies.EntityContext);
  const [value, setValue] = dependencies.useState(dependencies.simpleChangeHandler.getIntermediateValue(state, path));

  const copy = async () => {
    await navigator.clipboard.writeText(value);
  };

  const paste = async () => {
    const text = await navigator.clipboard.readText();

    setValue(text);
    dependencies.simpleChangeHandler.setValue(entityReference, path, text);
  };

  const onchange = newValue => {
    setValue(newValue != value ? newValue : null);
    context.impleChangeHandler.setValue(entityReference, path, newValue);
  }

  return dependencies.html`
    <input type="hidden" class="form-control" name=${name} value=${value} />

    ${value && dependencies.html`<div class="mb-2">
      <img class="media-picker-preview-image" src=${value} />
    </div>`}

    <${Dropdown} text="Add">
      <${MediaPickerMenu} provider=${provider} value=${value} context=${dependencies} onSelect=${onchange} />
    <//>

    <${Dropdown} text="Other" className="ms-2">
      <a class="dropdown-item" onClick=${ event => { copy(); closeDropdown(event.target); } }>Copy</a>
      <a class="dropdown-item" onClick=${ event => { paste(); closeDropdown(event.target); } }>Paste</a>
      <a class="dropdown-item" onClick=${ event => { setValue(null); dependencies.simpleChangeHandler.setValue(entityReference, path, null); closeDropdown(event.target); } }>Clear</a>
    <//>
  `;
};