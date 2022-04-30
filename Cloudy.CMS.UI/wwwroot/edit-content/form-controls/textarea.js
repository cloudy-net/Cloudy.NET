import { useContext } from '../../lib/preact.hooks.module.js';
import html from '../../util/html.js';
import { createRef } from '../../lib/preact.module.js';
import stateContext from '../state-context.js';

function Textarea({ fieldModel, initialValue, readonly, onchange }) {
    const state = useContext(stateContext);

    const ref = onchange && createRef();
    const changeEvent = (event) => (onchange(ref.current, event.srcElement.value));

    return html`
        <textarea
            ref=${ref}
            type="text"
            class="cloudy-ui-form-input"
            name=${fieldModel.descriptor.id}
            onInput=${changeEvent}
            defaultValue=${initialValue}
            rows=${fieldModel.descriptor.control.parameters.options && fieldModel.descriptor.control.parameters.options.rows ? fieldModel.descriptor.control.parameters.options.rows.value : 8}
            readonly=${readonly || state.loading}
        >
        <//>
    `;
}

export default Textarea;