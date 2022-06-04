import { useContext } from '../../lib/preact.hooks.module.js';
import fieldModelContext from './field-model-context.js';
import renderField from './render-field.js';

function EmbeddedForm({ path, formId, state }){
    const fieldModels = useContext(fieldModelContext)[formId];

    return fieldModels.map(fieldModel => renderField(fieldModel, state, [...path, fieldModel.descriptor.id]));
}

export default EmbeddedForm;