import ListFilterSelectDropdown from "./list-filter-select-dropdown";

export default ({ name, label, select, selectType, filter }) => {
    if (select) {
        return <>
            <ListFilterSelectDropdown label={label} contentType={selectType} onSelect={value => filter(name, value)} simpleKey={false} />
        </>;
    }
};