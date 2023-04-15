import { useEffect, useRef, useState} from 'preact/hooks';
import Dropdown from '../components/dropdown';

import SelectEntityMenu from "../components/select-entity-menu";

export default ({ label, entityType, onSelect, simpleKey }) => {
  const [value, setValue] = useState();

  return <div class="dropdown d-inline-block list-filter">
    {label}
    <Dropdown className="dropdown-button" contents={value && value.name}>
      <SelectEntityMenu entityType={entityType} simpleKey={simpleKey} value={value && value.reference} onSelect={item => { setValue(item); onSelect(item && item.reference); }} />
    </Dropdown>
  </div>;
};