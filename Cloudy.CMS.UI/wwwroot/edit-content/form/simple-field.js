
import html from '../../util/html.js';
import arrayEquals from "../../util/array-equals.js";

export default function SimpleField({ fieldModel, state, path }) {
    const getChangeBadge = () => {
        return html`<cloudy-ui-change-badge class=${state.simpleChanges && state.simpleChanges.find(ch => arrayEquals(ch.path, path)) ? 'cloudy-ui-unchanged' : null} title="This field has pending changes."><//>`;
    };

    return html`
        <div class="cloudy-ui-form-field cloudy-ui-simple">
            <div class="cloudy-ui-form-field-label">${fieldModel.descriptor.label || fieldModel.descriptor.id}${getChangeBadge()}</div>
            <${fieldModel.controlType} path=${path} fieldModel=${fieldModel} state=${state}/>
        </div>
    `;
}