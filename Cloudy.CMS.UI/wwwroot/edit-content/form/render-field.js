import html from '../../util/html.js';
import SimpleField from '../simple-field.js';
import SortableField from '../sortable-field.js';

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
        return html`<${EmbeddedForm}
            path=${path}
            fieldModel=${fieldModel}
            initialState=${initialState}
        />`;
    }

    return html`<${SimpleField}
        path=${path}
        fieldModel=${fieldModel}
        initialState=${initialState}
    />`;
}

export default renderField;