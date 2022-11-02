import { useEffect, useRef, useState } from 'preact/hooks';
import SelectOneDropdown from './select-one-dropdown';

export default ({ controlName, contentType, pageSize, value: initialValue, simpleKey, editLink, imageable }) => {
  const [value, setValue] = useState(initialValue);
  const [preview, setPreview] = useState();

  useEffect(() => {
    (async () => {
      if (!value) {
        return;
      }

      if (preview && preview.reference == value) {
        return;
      }

      var response = await fetch(`/Admin/api/controls/select/preview?contentType=${contentType}&reference=${value}&simpleKey=${simpleKey}`);

      if (response.status == 404) {
        setPreview({ notFound: true });
        return;
      }

      var json = await response.json();
      setPreview(json);
    })();
  }, [value]);

  return <>
    <input type="hidden" class="form-control" name={controlName} value={value} />

    {value && !preview && <div class={"input-group mb-3 select-one" + (imageable ? ' imageable' : '')}>
      <span class="input-group-text" ></span>
      <div class="form-control">&nbsp;</div>
    </div>}
    {preview && preview.notFound && <div class={"input-group mb-3 select-one" + (imageable ? ' imageable' : '')}>
      <div class="form-control"><span class="information-missing">Could not find <code>{simpleKey ? value : JSON.parse(value).join(', ')}</code></span></div>
      <button class="btn btn-beta" type="button" onClick={() => { setValue(null); setPreview(null); }}>Remove</button>
    </div>}
    {preview && !preview.notFound && <div class={"input-group mb-3 select-one" + (imageable ? ' imageable' : '')}>
      <span class="input-group-text" ></span>
      {preview.image && <img src={preview.image} class="select-one-preview-image" alt="" />}
      <div class="form-control">{preview.name}</div>
      <a class="btn btn-beta" href={`${editLink}&${simpleKey ? `keys=${preview.reference}` : preview.reference.map(key => `keys=${key}`).join('&')}`} target="_blank">Edit</a>
      <button class="btn btn-beta" type="button" onClick={() => { setValue(null); setPreview(null); }}>Remove</button>
    </div>}

    <SelectOneDropdown contentType={contentType} pageSize={pageSize} value={value} onSelect={item => { setValue(simpleKey ? item.reference : JSON.stringify(item.reference)); setPreview(item); }} simpleKey={simpleKey} imageable={imageable} />
  </>;
}
