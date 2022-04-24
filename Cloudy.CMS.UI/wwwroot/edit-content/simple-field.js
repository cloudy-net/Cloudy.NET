
import html from '../util/html.js';

export default function SimpleField({ path, fieldModel, readonly, initialValue }) {
    if (fieldModel.descriptor.embeddedFormId && !fieldModel.descriptor.isSortable) {
        wrapperTag = 'fieldset';
        labelTag = 'legend';
    }

    const emitEvent = (element, val) => element.dispatchEvent(new CustomEvent('cloudy-ui-form-change', { bubbles: true, detail: { change: { path, type: 'simple', operation: 'set', initialValue, value: val } } }))

    return html`
        <div class="cloudy-ui-form-field cloudy-ui-simple">
            <div class="cloudy-ui-form-field-label">${fieldModel.descriptor.label || fieldModel.descriptor.id}</div>
            <${fieldModel.controlType} onchange=${emitEvent} fieldModel=${fieldModel} initialValue=${initialValue} readonly=${readonly}/>
        </div>
    `;
}