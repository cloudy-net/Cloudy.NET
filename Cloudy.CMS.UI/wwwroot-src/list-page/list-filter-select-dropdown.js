import { html, useEffect, useRef, useState } from "../preact-htm/standalone.module.js";
import SelectEntityMenu from "../components/select-entity-menu.js";

export default ({ label, entityType, onSelect, simpleKey }) => {
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

  return html`<div class="dropdown d-inline-block list-filter" ref=${ref}>
    <div class="form-floating">
      <button class="form-select text-start" onClick=${() => setOpen(!open)}>${value && value.name}</button>
      <label>${label}</label>
    </div>
    <div class=${"dropdown-menu" + (open ? " show" : "")}>
      <${SelectEntityMenu} entityType=${entityType} simpleKey=${simpleKey} value=${value && value.reference} onSelect=${item => { setValue(item); onSelect(item && item.reference); }} />
    </div>
  </div>`;
};