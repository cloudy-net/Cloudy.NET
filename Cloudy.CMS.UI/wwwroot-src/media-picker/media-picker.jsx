import { useState } from "preact/hooks";
import ClickOutsideDetector from "../components/click-outside-detector";
import MediaPickerMenu from "./media-picker-menu";

export default ({ controlName, provider, initialValue }) => {
  const [value, setValue] = useState(initialValue);
  const [open, setOpen] = useState();

  return <>
    <input type="hidden" class="form-control" name={controlName} value={value} />

    {value && <div class="mb-2">
      <img src={value} style="width: 200px; max-height: 200px;" />
    </div>}

    <ClickOutsideDetector onClickOutside={() => setOpen(false)}>
      <div class="dropdown d-inline-block">
        <button class="btn btn-beta btn-sm dropdown-toggle" type="button" aria-expanded={open} onClick={() => setOpen(!open)}>
          Add
        </button>
        <div class={"dropdown-menu" + (open ? " show" : "")}>
          <MediaPickerMenu provider={provider} value={value} onSelect={newValue => { setValue(newValue != value ? newValue : null); setOpen(false); }} />
        </div>
      </div>
    </ClickOutsideDetector>
  </>;
};