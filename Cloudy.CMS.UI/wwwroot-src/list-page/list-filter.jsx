import ListFilterSelectDropdown from "./list-filter-select-dropdown";

export default ({ name, label, select, selectType, simpleKey, filter }) => {
    if (select) {
        return <>
            <ListFilterSelectDropdown label={label} contentType={selectType} simpleKey={simpleKey} onSelect={value => filter(name, value)} />
        </>;
    }
};