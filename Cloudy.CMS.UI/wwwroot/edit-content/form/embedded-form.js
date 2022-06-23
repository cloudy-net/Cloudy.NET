import html from '../../util/html.js';
import fieldDescriptorProvider from './field-descriptor-provider.js';
import renderField from './render-field.js';

function EmbeddedForm({ path, formId, state }){
    const fieldDescriptors = fieldDescriptorProvider.get(formId);

    return html`<div class=cloudy-ui-embedded-form>${fieldDescriptors.map(fieldDescriptor => renderField(fieldDescriptor, state, [...path, fieldDescriptor.id]))}<//>`;
}

export default EmbeddedForm;