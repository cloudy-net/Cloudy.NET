import html from '@src/html-init.js';
import ListFilterSelectDropdown from "./list-filter-select-dropdown.js";

export default ({ name, label, select, selectType, simpleKey, filter }) => {
    if (select) {
        return html`
            <${ListFilterSelectDropdown} label=${label} entityType=${selectType} simpleKey=${simpleKey} onSelect=${value => filter(name, value)} />
        `;
    }
};