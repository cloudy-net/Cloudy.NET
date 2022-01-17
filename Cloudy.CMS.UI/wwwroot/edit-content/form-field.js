
import html from '../util/html.js';
import SimpleField from './simple-field.js';
import SortableField from './sortable-field.js';

function FormField(props) {
    const { contentId, contentTypeId, value, fieldModel } = props;
    const path = [fieldModel.descriptor.id];

    if (fieldModel.descriptor.isSortable) {
        return html`<${SortableField}
            contentId=${contentId}
            contentTypeId=${contentTypeId}
            value=${value}
            path=${path}
            fieldModel=${fieldModel}
            readonly=${props.readonly}
        />`;
    }

    if (fieldModel.descriptor.embeddedFormId) {
        return html`<${EmbeddedForm}
            contentId=${contentId}
            contentTypeId=${contentTypeId}
            value=${value}
            path=${path}
            fieldModel=${fieldModel}
            readonly=${props.readonly}
        />`;
    }

    return html`<${SimpleField}
        contentId=${contentId}
        contentTypeId=${contentTypeId}
        value=${value}
        path=${path}
        fieldModel=${fieldModel}
        readonly=${props.readonly}
    />`;
}

export default FormField;
