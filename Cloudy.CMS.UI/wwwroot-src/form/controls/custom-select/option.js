import { html } from '../../../preact-htm/standalone.module.js';

export default ({ option, isMultiSelect, initialValue, isCreateMode }) => html
`<option
    disabled=${option.disabled}
    value=${option.value}
    selected=${isMultiSelect
        ? initialValue.includes(option.value) || (isCreateMode && option.selected)
        : initialValue == option.value || (isCreateMode && option.selected)}>${option.text}
</option>`;
