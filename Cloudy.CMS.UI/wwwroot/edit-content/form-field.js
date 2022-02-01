
import html from '../util/html.js';
import SimpleField from './simple-field.js';
import SortableField from './sortable-field.js';

function FormField({ contentId, contentTypeId, initialValue, fieldModel, readonly }) {
    const path = [fieldModel.descriptor.id];

    if (fieldModel.descriptor.isSortable) {
        return html`<${SortableField}
            contentId=${contentId}
            contentTypeId=${contentTypeId}
            initialValue=${initialValue}
            path=${path}
            fieldModel=${fieldModel}
            readonly=${readonly}
        />`;
    }

    if (fieldModel.descriptor.embeddedFormId) {
        return html`<${EmbeddedForm}
            contentId=${contentId}
            contentTypeId=${contentTypeId}
            initialValue=${initialValue}
            path=${path}
            fieldModel=${fieldModel}
            readonly=${readonly}
        />`;
    }

    return html`<${SimpleField}
        contentId=${contentId}
        contentTypeId=${contentTypeId}
        initialValue=${initialValue}
        path=${path}
        fieldModel=${fieldModel}
        readonly=${readonly}
    />`;
}

export default FormField;
