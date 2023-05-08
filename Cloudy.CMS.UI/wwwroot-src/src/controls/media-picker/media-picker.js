import MediaPickerMenu from "./media-picker-menu.js";

export default ({ name, path, provider, dependencies }) => {
  const {
    html,
    useContext,
    useState,
    simpleChangeHandler,
    EntityContext,
    Dropdown,
    DropdownItem,
    closeDropdown,
  } = dependencies;

  const { entityReference, state } = useContext(EntityContext);
  const [value, setValue] = useState(simpleChangeHandler.getIntermediateValue(state, path));

  const copy = async () => {
    await navigator.clipboard.writeText(value);
  };

  const paste = async () => {
    const text = await navigator.clipboard.readText();

    setValue(text);
    simpleChangeHandler.setValue(entityReference, path, text);
  };

  const onchange = newValue => {
    setValue(newValue != value ? newValue : null);
    simpleChangeHandler.setValue(entityReference, path, newValue);
  }

  return html`
    ${value && html`<div class="mb5">
      <img class="media-picker-preview-image" src=${value} />
    </div>`}

    <${Dropdown} wideContent=${true} contents=${value ? decodeURIComponent(value.split("/").slice(-1)) : "Add"}>
      <${MediaPickerMenu} provider=${provider} value=${value} onSelect=${onchange} dependencies=${dependencies} />
    <//>

    <${Dropdown} contents="Other" className="button ml5">
      <${DropdownItem} text="Copy" onClick=${ event => { copy(); closeDropdown(event.target); } } />
      <${DropdownItem} text="Paste" onClick=${ event => { paste(); closeDropdown(event.target); } } />
      <${DropdownItem} text="Clear" onClick=${ event => { setValue(null); simpleChangeHandler.setValue(entityReference, path, null); closeDropdown(event.target); } } />
    <//>
  `;
};