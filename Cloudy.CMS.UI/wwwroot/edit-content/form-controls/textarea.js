import html from '../../util/html.js';
import { createRef } from '../../lib/preact.module.js';
import propertyGetter from '../../data/property-getter.js';

function Textarea({ fieldModel, initialState, path, readonly, onchange }) {
    const ref = onchange && createRef();
    const changeEvent = (event) => (onchange(ref.current, event.srcElement.value));

    return html`
        <textarea
            ref=${ref}
            type="text"
            class="cloudy-ui-form-input"
            name=${fieldModel.descriptor.id}
            onInput=${changeEvent}
            defaultValue=${propertyGetter.get(initialState, path)}
            rows=${fieldModel.descriptor.control.parameters.options && fieldModel.descriptor.control.parameters.options.rows ? fieldModel.descriptor.control.parameters.options.rows.value : 8}
            readonly=${readonly}
        >
        <//>
    `;
}

export default Textarea;