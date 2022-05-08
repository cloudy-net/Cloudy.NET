import propertyGetter from '../../data/property-getter.js';
import { createRef } from '../../lib/preact.module.js';
import html from '../../util/html.js';

function Text({ fieldModel, initialState, path, onchange, readonly }) {
    const ref = onchange && createRef();
    const changeEvent = onchange && (event => onchange(ref.current, event.srcElement.value));

    return html`
        <input
            ref=${ref}
            type="text"
            class="cloudy-ui-form-input"
            name=${fieldModel.descriptor.id}
            defaultValue=${propertyGetter.get(initialState, path)}
            onInput=${changeEvent}
            readonly=${readonly}
        />
    `;
}

export default Text;