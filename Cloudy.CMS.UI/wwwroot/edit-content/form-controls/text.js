import { createRef } from '../../lib/preact.module.js';
import getIntermediateValue from '../../util/get-intermediate-value.js';
import html from '../../util/html.js';

function Text({ fieldModel, initialState, path, onchange, readonly }) {
    const ref = onchange && createRef();
    const changeEvent = onchange && (event => onchange(ref.current, event.srcElement.value));

    return html`
        <input
            ref=${ref}
            type="text"
            key=${initialState.contentReference}
            class="cloudy-ui-form-input"
            name=${fieldModel.descriptor.id}
            defaultValue=${getIntermediateValue(initialState.referenceValues, path, initialState.changes)}
            onInput=${changeEvent}
            readonly=${readonly}
        />
    `;
}

export default Text;