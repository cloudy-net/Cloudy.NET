import fieldDescriptorProvider from './field-descriptor-provider.js';
import renderField from './render-field.js';

function EmbeddedForm({ path, formId, state }){
    const fieldDescriptors = fieldDescriptorProvider.get(formId);

    return fieldDescriptors.map(fieldDescriptor => renderField(fieldDescriptor, state, [...path, fieldDescriptor.id]));
}

export default EmbeddedForm;