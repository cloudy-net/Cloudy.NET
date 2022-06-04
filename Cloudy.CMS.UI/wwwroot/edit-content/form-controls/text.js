import getIntermediateSimpleValue from '../../util/get-intermediate-simple-value.js';
import html from '../../util/html.js';
import stateManager from '../state-manager.js';

function Text({ fieldDescriptor, state, path, readonly }) {
    return html`
        <input
            type="text"
            key=${state.contentReference}
            class="cloudy-ui-form-input"
            name=${fieldDescriptor.id}
            defaultValue=${getIntermediateSimpleValue(state.referenceValues, path, state.simpleChanges)}
            onInput=${event => stateManager.registerSimpleChange(state.contentReference, path, event.srcElement.value)}
            readonly=${readonly}
        />
    `;
}

export default Text;