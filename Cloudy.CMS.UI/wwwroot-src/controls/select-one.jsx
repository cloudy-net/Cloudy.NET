import { useEffect, useRef, useState } from 'preact/hooks';
import SelectOneDropdown from './select-one-dropdown';

export default ({ controlName, contentType, pageSize, value: initialValue }) => {
  const [open, setOpen] = useState();
  const [value, setValue] = useState(initialValue);
  const [item, setItem] = useState();
  const ref = useRef(null);

  useEffect(() => {
    if(!value){
      return;
    }

    if(item && item.reference == value){
      return;
    }

    fetch(`/Admin/api/controls/select/preview?contentType=${contentType}&reference=${value}`)
      .then(response => response.json())
      .then(response => {
        setItem(response);
      });
  }, [value]);

  // useEffect(() => {
  //   const callback = event => {
  //     if(!ref.current){
  //       return;
  //     }
  //     if(ref.current == event.target || ref.current.contains(event.target)){
  //       return;
  //     }
  //     setOpen(false);
  //   };
  //   document.addEventListener('click', callback);
  //   return () => document.removeEventListener('click', callback);
  // }, []);

  return <>
    <input type="hidden" class="form-control" name={controlName} value={value} />

    {item && <div class="form-control mb-2">{item.name}</div>}

    <div class="dropdown" ref={ref}>
      <button class="btn btn-beta dropdown-toggle" type="button" aria-expanded={open} onClick={() => setOpen(!open)}>Add</button>
      <div class={"dropdown-menu" + (open ? " show" : "")}>
        <SelectOneDropdown contentType={contentType} pageSize={pageSize} value={value} onSelect={item => { setValue(item.reference); setItem(item); setOpen(false); }} />
      </div>
    </div>
  </>;
}
