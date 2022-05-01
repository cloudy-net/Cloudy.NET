
import html from '../util/html.js';
import SimpleField from './simple-field.js';
import SortableField from './sortable-field.js';

function FormField({ fieldModel, state }) {
    const path = [fieldModel.descriptor.id];

    if (fieldModel.descriptor.isSortable) {
        return html`<${SortableField}
            path=${path}
            fieldModel=${fieldModel}
            state=${state}
        />`;
    }

    if (fieldModel.descriptor.embeddedFormId) {
        return html`<${EmbeddedForm}
            path=${path}
            fieldModel=${fieldModel}
            state=${state}
        />`;
    }

    return html`<${SimpleField}
        path=${path}
        fieldModel=${fieldModel}
        state=${state}
    />`;
}

export default FormField;
