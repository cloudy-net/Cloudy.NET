import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import pendingChangesContext from '../diff/pending-changes-context.js';

function SimpleField({ contentId, contentTypeId, path, fieldModel, readonly, value: initialValue }) {
    const [, , , getPendingValue] = useContext(pendingChangesContext);
    const [pendingValue, setPendingValue] = useState();

    useEffect(() => {
        setPendingValue(getPendingValue(contentId, contentTypeId, path, initialValue));
    }, [contentId, contentTypeId, path, initialValue]);

    if (fieldModel.descriptor.embeddedFormId && !fieldModel.descriptor.isSortable) {
        wrapperTag = 'fieldset';
        labelTag = 'legend';
    }

    const emitEvent = (element, val) => element.dispatchEvent(new CustomEvent('cloudy-ui-form-change', { bubbles: true, detail: { change: { path, type: 'simple', operation: 'set', initialValue, value: val } } }))

    return html`
        <div class="cloudy-ui-form-field cloudy-ui-simple">
            <div class="cloudy-ui-form-field-label">${fieldModel.descriptor.label || fieldModel.descriptor.id}</div>
            <${fieldModel.controlType} onchange=${emitEvent} fieldModel=${fieldModel} initialValue=${pendingValue} readonly=${readonly}/>
        </div>
    `;
}

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
