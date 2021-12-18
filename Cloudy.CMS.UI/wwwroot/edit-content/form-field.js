import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import changeTracker from './change-tracker.js';

function SimpleField(props) {
    const { contentId, contentTypeId, path, fieldModel } = props;
    const initialValue = props.value;

    const pendingValue = changeTracker.getPendingValue(contentId, contentTypeId, path, initialValue);

    let wrapperTag = 'div';
    let labelTag = 'div';

    if (fieldModel.descriptor.embeddedFormId && !fieldModel.descriptor.isSortable) {
        wrapperTag = 'fieldset';
        labelTag = 'legend';
    }

    const emitEvent = (element, value) => element.dispatchEvent(new CustomEvent('cloudy-ui-form-change', { bubbles: true, detail: { change: { path, type: 'simple', operation: 'set', initialValue, value } } }))

    return html`
        <${wrapperTag} class="cloudy-ui-form-field cloudy-ui-simple">
            <${labelTag} class="cloudy-ui-form-field-label">${fieldModel.descriptor.label || fieldModel.descriptor.id}<//>
            <${fieldModel.controlType} onchange=${emitEvent} fieldModel=${fieldModel} initialValue=${pendingValue}/>
        <//>
    `;
}

function FormField(props) {
    const { contentId, contentTypeId, value, fieldModel } = props;
    const path = [...props.path, fieldModel.descriptor.id];

    if (fieldModel.descriptor.isSortable) {
        return html`<${SortableField}
            contentId=${contentId}
            contentTypeId=${contentTypeId}
            value=${value}
            path=${path}
            fieldModel=${fieldModel}
        />`;
    }

    if (fieldModel.descriptor.embeddedFormId) {
        return html`<${EmbeddedForm}
            contentId=${contentId}
            contentTypeId=${contentTypeId}
            value=${value}
            path=${path}
            fieldModel=${fieldModel}
        />`;
    }

    return html`<${SimpleField}
        contentId=${contentId}
        contentTypeId=${contentTypeId}
        value=${value}
        path=${path}
        fieldModel=${fieldModel}
    />`;
}

export default FormField;
