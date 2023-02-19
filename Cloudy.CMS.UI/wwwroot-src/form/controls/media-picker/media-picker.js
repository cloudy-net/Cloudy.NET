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
    dependencies.simpleChangeHandler.setValue(entityReference, path, newValue);
  }

  return dependencies.html`
    <input type="hidden" class="form-control" name=${name} value=${value} />

    ${value && dependencies.html`<div class="mb-2">
      <img class="media-picker-preview-image" src=${value} />
    </div>`}

    <${dependencies.Dropdown} text="Add">
      <${MediaPickerMenu} provider=${provider} value=${value} onSelect=${onchange} dependencies=${dependencies} />
    <//>

    <${dependencies.Dropdown} text="Other" className="ms-2">
      <a class="dropdown-item" onClick=${ event => { copy(); dependencies.closeDropdown(event.target); } }>Copy</a>
      <a class="dropdown-item" onClick=${ event => { paste(); dependencies.closeDropdown(event.target); } }>Paste</a>
      <a class="dropdown-item" onClick=${ event => { setValue(null); dependencies.simpleChangeHandler.setValue(entityReference, path, null); dependencies.closeDropdown(event.target); } }>Clear</a>
    <//>
  `;
};