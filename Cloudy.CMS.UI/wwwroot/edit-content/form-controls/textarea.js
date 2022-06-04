import html from '../../util/html.js';
import getIntermediateSimpleValue from '../../util/get-intermediate-simple-value.js';
import stateManager from '../state-manager.js';

function Textarea({ fieldModel, state, path, readonly }) {
    return html`
        <textarea
            type="text"
            class="cloudy-ui-form-input"
            name=${fieldModel.descriptor.id}
            onInput=${event => stateManager.registerSimpleChange(state.contentReference, path, event.srcElement.value)}
            defaultValue=${getIntermediateSimpleValue(state.referenceValues, path, state.simpleChanges)}
            rows=${fieldModel.descriptor.control.parameters.options && fieldModel.descriptor.control.parameters.options.rows ? fieldModel.descriptor.control.parameters.options.rows.value : 8}
            readonly=${readonly}
        >
        <//>
    `;
}

export default Textarea;