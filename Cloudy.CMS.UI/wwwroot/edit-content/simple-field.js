
import html from '../util/html.js';
import arrayEquals from "../util/array-equals.js";

export default function SimpleField({ fieldModel, initialState, path }) {
    const emitEvent = (element, value) => element.dispatchEvent(new CustomEvent('cloudy-ui-form-change', { bubbles: true, detail: { change: { path, type: 'simple', operation: 'set', value } } }))

    const getChangeBadge = () => {
        return html`<cloudy-ui-change-badge class=${initialState.changes && initialState.changes.find(ch => arrayEquals(ch.path, path)) ? 'cloudy-ui-unchanged' : null} title="This field has pending changes."><//>`;
    };

    return html`
        <div class="cloudy-ui-form-field cloudy-ui-simple">
            <div class="cloudy-ui-form-field-label">${fieldModel.descriptor.label || fieldModel.descriptor.id}${getChangeBadge()}</div>
            <${fieldModel.controlType} onchange=${emitEvent} path=${path} fieldModel=${fieldModel} initialState=${initialState}/>
        </div>
    `;
}