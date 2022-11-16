import { useState } from "preact/hooks";
import closeDropdown from "../components/close-dropdown";
import Dropdown from "../components/dropdown";
import MediaPickerMenu from "./media-picker-menu";

export default ({ controlName, provider, initialValue }) => {
  const [value, setValue] = useState(initialValue);

  const copy = async () => {
    await navigator.clipboard.writeText(value);
  };

  const paste = async () => {
    const text = await navigator.clipboard.readText();

    setValue(text);
  };

  return <>
    <input type="hidden" class="form-control" name={controlName} value={value} />

    {value && <div class="mb-2">
      <img class="media-picker-preview-image" src={value} />
    </div>}

    <Dropdown text="Add">
      <MediaPickerMenu provider={provider} value={value} onSelect={newValue => { setValue(newValue != value ? newValue : null); }} />
    </Dropdown>

    <Dropdown text="Other" className="ms-2">
      <a class="dropdown-item" onClick={ event => { copy(); closeDropdown(event.target); } }>Copy</a>
      <a class="dropdown-item" onClick={ event => { paste(); closeDropdown(event.target); } }>Paste</a>
      <a class="dropdown-item" onClick={ event => { setValue(''); closeDropdown(event.target); } }>Clear</a>
    </Dropdown>
  </>;
};