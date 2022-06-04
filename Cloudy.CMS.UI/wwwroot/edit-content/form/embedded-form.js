import { useContext } from '../../lib/preact.hooks.module.js';
import fieldDescriptorContext from './field-descriptor-context.js';
import renderField from './render-field.js';

function EmbeddedForm({ path, formId, state }){
    const fieldDescriptors = useContext(fieldDescriptorContext)[formId];

    return fieldDescriptors.map(fieldDescriptor => renderField(fieldDescriptor, state, [...path, fieldDescriptor.id]));
}

export default EmbeddedForm;