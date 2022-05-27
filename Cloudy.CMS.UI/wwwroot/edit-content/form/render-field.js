import arrayEquals from '../../util/array-equals.js';
import html from '../../util/html.js';
import SimpleField from '../simple-field.js';
import SortableField from '../sortable-field.js';
import EmbeddedForm from './embedded-form.js';

const renderField = (fieldModel, initialState, path) => {
    if(!initialState || initialState.loading || initialState.loadingNewVersion){
        return;
    }

    if (fieldModel.descriptor.isSortable) {
        return html`<${SortableField}
            path=${path}
            fieldModel=${fieldModel}
            initialState=${initialState}
        />`;
    }

    if (fieldModel.descriptor.embeddedFormId) {
        const getChangeBadge = () => {
            return html`<cloudy-ui-change-badge class=${initialState.changedFields && initialState.changedFields.find(ch => arrayEquals(ch.path, path)) ? 'cloudy-ui-unchanged' : null} title="This field has pending changes."><//>`;
        };
    
        return html`
            <fieldset class="cloudy-ui-form-field">
                <legend class="cloudy-ui-form-field-label">${fieldModel.descriptor.label || fieldModel.descriptor.id}${getChangeBadge()}<//>
                <${EmbeddedForm}
                    path=${path}
                    formId=${fieldModel.descriptor.embeddedFormId}
                    initialState=${initialState}
                />
            <//>
        `;
    }

    return html`<${SimpleField}
        path=${path}
        fieldModel=${fieldModel}
        initialState=${initialState}
    />`;
}

export default renderField;