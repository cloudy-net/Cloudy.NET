
import html from '../../util/html.js';
import arrayEquals from "../../util/array-equals.js";
import { useContext } from '../../lib/preact.hooks.module.js';
import fieldControlContext from './field-control-context.js';
import getReferenceValue from '../../util/get-reference-value.js';
import getIntermediateSimpleValue from '../../util/get-intermediate-simple-value.js';

export default function SimpleField({ fieldDescriptor, state, path }) {
    const fieldControl = useContext(fieldControlContext)[fieldDescriptor.control && fieldDescriptor.control.id];

    const getChangeIndicator = () => {
        const referenceValue = getReferenceValue(state, path);
        const intermediateValue = getIntermediateSimpleValue(state, path);

        if(!referenceValue && intermediateValue){
            return html`<cloudy-ui-form-field-change-indicator class=cloudy-ui-added title="This value has been added."><//>`;
        }

        if(referenceValue && !intermediateValue){
            return html`<cloudy-ui-form-field-change-indicator class=cloudy-ui-removed title="This value has been removed."><//>`;
        }

        if(referenceValue != intermediateValue){
            return html`<cloudy-ui-form-field-change-indicator class=cloudy-ui-modified title="This value has been modified."><//>`;
        }
    };

    if(!fieldControl){
        return html`
            <div class="cloudy-ui-form-field cloudy-ui-simple">
                <div class="cloudy-ui-form-field-label">${fieldDescriptor.label || fieldDescriptor.id}${getChangeIndicator()}</div>
                <cloudy-ui-info-message>Error: Control ${fieldDescriptor.control && fieldDescriptor.control.id} not found<//>
            </div>
        `;
    }

    return html`
        <label class="cloudy-ui-form-field cloudy-ui-simple">
            <div class="cloudy-ui-form-field-label">${fieldDescriptor.label || fieldDescriptor.id}${getChangeIndicator()}<//>
            <${fieldControl} path=${path} fieldDescriptor=${fieldDescriptor} state=${state}/>
        <//>
    `;
}