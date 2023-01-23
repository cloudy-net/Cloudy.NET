import { html } from '../../../preact-htm/standalone.module.js';

export default ({ option, isMultiSelect, initialValue, preselect }) => html
`<option
    disabled=${option.disabled}
    value=${option.value}
    selected=${isMultiSelect
        ? initialValue.includes(option.value) || (preselect && option.selected)
        : initialValue == option.value || (preselect && option.selected)}>${option.text}
</option>`;
