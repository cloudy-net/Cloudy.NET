import html from '../util/html.js';

function EmbeddedForm({ path, formId, initialState }){
    const fieldModels = useContext(fieldModelContext)[formId];

    return html`
        <div class='cloudy-ui-form ${state.loading || state.loadingNewVersion ? 'cloudy-ui-loading' : null}' ref=${ref}>
            ${fieldModels.map(fieldModel => renderField(fieldModel, initialState, [...path, fieldModel.descriptor.id]))}
        <//>
    `;
}

export default EmbeddedForm;