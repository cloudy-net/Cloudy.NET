import { useEffect, useRef, useState } from 'preact/hooks';
import SelectOneDropdown from './select-one-dropdown';

export default ({ controlName, contentType, pageSize, value: initialValue, simpleKey }) => {
  const [value, setValue] = useState(initialValue);
  const [preview, setPreview] = useState();

  useEffect(() => {
    if (!value) {
      return;
    }

    if (preview && preview.reference == value) {
      return;
    }

    fetch(`/Admin/api/controls/select/preview?contentType=${contentType}&reference=${value}&simpleKey=${simpleKey}`)
      .then(response => response.json())
      .then(response => {
        setPreview(response);
      });
  }, [value]);

  return <>
    <input type="hidden" class="form-control" name={controlName} value={value} />

    {value && !preview && <div class="input-group mb-3">
      <span class="input-group-text" ></span>
      <div type="text" class="form-control">&nbsp;</div>
    </div>}
    {preview && <div class="input-group mb-3">
      <span class="input-group-text" ></span>
      <div type="text" class="form-control">{preview.name}</div>
    </div>}

    <SelectOneDropdown contentType={contentType} pageSize={pageSize} value={value} onSelect={item => { setValue(simpleKey ? item.reference : JSON.stringify(item.reference)); setPreview(item); }} simpleKey={simpleKey} />
  </>;
}
