import { useEffect, useRef, useState} from 'preact/hooks';
import Dropdown from '../components/dropdown';

import SelectEntityMenu from "../components/select-entity-menu";

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

  return <div class="dropdown d-inline-block list-filter" ref={ref}>
    {label}
    <Dropdown contents={value && value.name}>
      <SelectEntityMenu entityType={entityType} simpleKey={simpleKey} value={value && value.reference} onSelect={item => { setValue(item); onSelect(item && item.reference); }} />
    </Dropdown>
  </div>;
};