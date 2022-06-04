import arrayEquals from '../../util/array-equals.js';
import html from '../../util/html.js';
import SimpleField from './simple-field.js';
import SortableField from './sortable-field.js';
import EmbeddedForm from './embedded-form.js';

const renderField = (fieldDescriptor, state, path) => {
    if(!state || state.loading || state.loadingNewVersion){
        return;
    }

    if (fieldDescriptor.isSortable) {
        return html`<${SortableField}
            path=${path}
            fieldDescriptor=${fieldDescriptor}
            state=${state}
        />`;
    }

    if (fieldDescriptor.embeddedFormId) {
        const getChangeBadge = () => {
            return html`<cloudy-ui-change-badge class=${state.simpleChanges && state.simpleChanges.find(ch => arrayEquals(ch.path, path)) ? 'cloudy-ui-unchanged' : null} title="This field has pending changes."><//>`;
        };
    
        return html`
            <fieldset class="cloudy-ui-form-field">
                <legend class="cloudy-ui-form-field-label">${fieldDescriptor.label || fieldDescriptor.id}${getChangeBadge()}<//>
                <${EmbeddedForm}
                    path=${path}
                    formId=${fieldDescriptor.embeddedFormId}
                    state=${state}
                />
            <//>
        `;
    }

    return html`<${SimpleField}
        path=${path}
        fieldDescriptor=${fieldDescriptor}
        state=${state}
    />`;
}

export default renderField;