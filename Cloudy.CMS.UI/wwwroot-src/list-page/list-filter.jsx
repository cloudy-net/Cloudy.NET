import ListFilterSelectDropdown from "./list-filter-select-dropdown";
import { useState } from "preact/hooks";

export default ({ name, label, select, selectType, simpleKey, filter }) => {
    const [value, setValue] = useState();
    if (select) {
        return <ListFilterSelectDropdown label={label} entityType={selectType} simpleKey={simpleKey} onSelect={value => filter(name, value)} />;
    }
};