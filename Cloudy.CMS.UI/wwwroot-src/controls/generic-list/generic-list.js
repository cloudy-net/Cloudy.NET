export default ({ name, path, provider, dependencies }) => {
  const {
    html,
    useContext,
    useState,
    simpleChangeHandler,
    EntityContext,
    Dropdown,
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
    <input type="hidden" class="form-control" name=${dependencies.componentContextProvider.getIdentifier(path)} value=${value} />

    ${value && html`<div class="mb-2">
      <img class="media-picker-preview-image" src=${value} />
    </div>`}

    <${Dropdown} text="Add">
    hej
    <//>

    <${Dropdown} text="Other" className="ms-2">
      <a class="dropdown-item" onClick=${ event => { copy(); closeDropdown(event.target); } }>Copy</a>
      <a class="dropdown-item" onClick=${ event => { paste(); closeDropdown(event.target); } }>Paste</a>
      <a class="dropdown-item" onClick=${ event => { setValue(null); simpleChangeHandler.setValue(entityReference, path, null); closeDropdown(event.target); } }>Clear</a>
    <//>
  `;
};