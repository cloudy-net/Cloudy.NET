import { useEffect, useRef, useState } from 'preact/hooks';
import SelectOneDropdown from './select-one-dropdown';
import SelectOneFilter from './select-one-filter';

export default ({ controlName, contentType, pageSize, value: initialValue }) => {
  const [value, setValue] = useState(initialValue);
  const [preview, setPreview] = useState();

  useEffect(() => {
    if(!value){
      return;
    }

    if(preview && preview.reference == value){
      return;
    }

    fetch(`/Admin/api/controls/select/preview?contentType=${contentType}&reference=${value}`)
      .then(response => response.json())
      .then(response => {
        setPreview(response);
      });
  }, [value]);

  return <>
    <input type="hidden" class="form-control" name={controlName} value={value} />

    {preview && <div class="form-control mb-2">{preview.name}</div>}

    <SelectOneDropdown contentType={contentType} pageSize={pageSize} value={value} onSelect={item => { setValue(item.reference); setPreview(item); }} />
  </>;
}
