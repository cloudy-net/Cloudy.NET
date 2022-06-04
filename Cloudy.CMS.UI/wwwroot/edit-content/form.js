import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import stateContext from './state-context.js';
import renderField from './form/render-field.js';
import fieldModelContext from './form/field-model-context.js';

function Form({ contentReference }) {
    const fieldModels = useContext(fieldModelContext)[contentReference.contentTypeId];

    const state = useContext(stateContext);

    const [initialState, setInitialState] = useState();

    useEffect(() => {
        setInitialState(state);
    }, [state.contentReference, state.loading, state.referenceValues, state.loadingNewVersion, state.simpleChanges]);

    return html`
        <div class='cloudy-ui-form ${state.loading || state.loadingNewVersion ? 'cloudy-ui-loading' : null}'>
            ${fieldModels.map(fieldModel => renderField(fieldModel, initialState, [fieldModel.descriptor.id]))}
        <//>
    `;
}

export default Form;
