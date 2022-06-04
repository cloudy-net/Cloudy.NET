import html from '../../util/html.js';
import getIntermediateSimpleValue from '../../util/get-intermediate-simple-value.js';
import stateManager from '../state-manager.js';

function Textarea({ fieldDescriptor, state, path, readonly }) {
    return html`
        <textarea
            type="text"
            class="cloudy-ui-form-input"
            name=${fieldDescriptor.id}
            onInput=${event => stateManager.registerSimpleChange(state.contentReference, path, event.srcElement.value)}
            defaultValue=${getIntermediateSimpleValue(state.referenceValues, path, state.simpleChanges)}
            rows=${fieldDescriptor.control.parameters.options && fieldDescriptor.control.parameters.options.rows ? fieldDescriptor.control.parameters.options.rows.value : 8}
            readonly=${readonly}
        >
        <//>
    `;
}

export default Textarea;