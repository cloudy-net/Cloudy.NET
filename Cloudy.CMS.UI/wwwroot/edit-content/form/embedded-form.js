import { useContext } from '../../lib/preact.hooks.module.js';
import html from '../../util/html.js';
import fieldModelContext from './field-model-context.js';
import renderField from './render-field.js';

function EmbeddedForm({ path, formId, initialState }){
    const fieldModels = useContext(fieldModelContext)[formId];

    return html`
        <cloudy-ui-sortable-item-form>
            ${fieldModels.map(fieldModel => renderField(fieldModel, initialState, [...path, fieldModel.descriptor.id]))}
        <//>
    `;
}

export default EmbeddedForm;