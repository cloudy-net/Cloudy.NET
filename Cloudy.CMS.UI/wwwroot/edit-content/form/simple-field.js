
import html from '../../util/html.js';
import arrayEquals from "../../util/array-equals.js";
import { useContext } from '../../lib/preact.hooks.module.js';
import fieldControlContext from './field-control-context.js';

export default function SimpleField({ fieldDescriptor, state, path }) {
    const fieldControl = useContext(fieldControlContext)[fieldDescriptor.control && fieldDescriptor.control.id];

    const getChangeBadge = () => {
        return html`<cloudy-ui-change-badge class=${state.simpleChanges && state.simpleChanges.find(ch => arrayEquals(ch.path, path)) ? 'cloudy-ui-unchanged' : null} title="This field has pending changes."><//>`;
    };

    if(!fieldControl){
        return html`
            <div class="cloudy-ui-form-field cloudy-ui-simple">
                <div class="cloudy-ui-form-field-label">${fieldDescriptor.label || fieldDescriptor.id}${getChangeBadge()}</div>
                <cloudy-ui-info-message>Error: Control ${fieldDescriptor.control && fieldDescriptor.control.id} not found<//>
            </div>
        `;
    }

    return html`
        <div class="cloudy-ui-form-field cloudy-ui-simple">
            <div class="cloudy-ui-form-field-label">${fieldDescriptor.label || fieldDescriptor.id}${getChangeBadge()}</div>
            <${fieldControl} path=${path} fieldDescriptor=${fieldDescriptor} state=${state}/>
        </div>
    `;
}