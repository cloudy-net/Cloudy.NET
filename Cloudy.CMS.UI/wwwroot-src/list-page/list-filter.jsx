import ListFilterSelectDropdown from "./list-filter-select-dropdown";
import Dropdown from '../components/dropdown';
import SelectEntityMenu from "../components/select-entity-menu";
import { useState } from "preact/hooks";

export default ({ name, label, select, selectType, simpleKey, filter }) => {
    const [value, setValue] = useState();
    if (select) {
        return <Dropdown text="">
            <SelectEntityMenu entityType={selectType} simpleKey={simpleKey} value={value && value.reference} onSelect={item => { setValue(item); onSelect(item && item.reference); }} />
        </Dropdown>
        return <ListFilterSelectDropdown label={label} entityType={selectType} simpleKey={simpleKey} onSelect={value => filter(name, value)} />;
    }
};