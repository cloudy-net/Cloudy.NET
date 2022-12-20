import { useEffect, useRef, useState } from "@preact-htm";
import SelectEntityMenu from "../components/select-entity-menu";

export default ({ label, contentType, onSelect, simpleKey }) => {
  const [value, setValue] = useState();
  const [open, setOpen] = useState();
  const ref = useRef(null);

  useEffect(() => {
    const callback = event => {
      if (!ref.current) {
        return;
      }
      if (ref.current == event.target || ref.current.contains(event.target)) {
        return;
      }
      setOpen(false);
    };
    document.addEventListener('click', callback);
    return () => document.removeEventListener('click', callback);
  }, []);

  return <div class="dropdown d-inline-block list-filter" ref={ref}>
    <div class="form-floating">
      <button class="form-select text-start" onClick={() => setOpen(!open)}>{value && value.name}</button>
      <label>{label}</label>
    </div>
    <div class={"dropdown-menu" + (open ? " show" : "")}>
      <SelectEntityMenu contentType={contentType} simpleKey={simpleKey} value={value && value.reference} onSelect={item => { setValue(item); onSelect(item && item.reference); }} />
    </div>
  </div>;
};