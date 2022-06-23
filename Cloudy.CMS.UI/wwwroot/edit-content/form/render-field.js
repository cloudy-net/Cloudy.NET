import html from '../../util/html.js';
import SimpleField from './simple-field.js';
import SortableField from './sortable-field.js';
import EmbeddedForm from './embedded-form.js';
import getReferenceValue from '../../util/get-reference-value.js';
import getIntermediateSimpleValue from '../../util/get-intermediate-simple-value.js';

const renderField = (fieldDescriptor, state, path) => {
    if(!state || state.loading || state.loadingNewVersion){
        return;
    }

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

    const wrap = element => html`
        <div class="cloudy-ui-form-field">
            <div class="cloudy-ui-form-field-label">${fieldDescriptor.label || fieldDescriptor.id}${getChangeIndicator()}</div>
            ${element}
        </div>
    `;

    if (fieldDescriptor.isSortable) {
        return wrap(html`<${SortableField}
            path=${path}
            fieldDescriptor=${fieldDescriptor}
            state=${state}
        />`);
    }

    if (fieldDescriptor.embeddedFormId) {
        return wrap(html`
            <${EmbeddedForm}
                path=${path}
                formId=${fieldDescriptor.embeddedFormId}
                state=${state}
            />
        `);
    }

    return wrap(html`<${SimpleField}
        path=${path}
        fieldDescriptor=${fieldDescriptor}
        state=${state}
    />`);
}

export default renderField;