import { html } from '../../../preact-htm/standalone.module.js';

export default ({ option, isMultiSelect, value, preselect }) => html
`<option
    disabled=${option.disabled}
    value=${option.value}
    selected=${isMultiSelect
        ? value.includes(option.value) || (preselect && option.selected)
        : value == option.value || (preselect && option.selected)}>${option.text}
</option>`;
