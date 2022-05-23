import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import { createRef } from '../lib/preact.module.js';
import stateManager from './state-manager.js';
import stateContext from './state-context.js';
import renderField from './form/render-field.js';
import fieldModelContext from './form/field-model-context.js';

function Form({ contentReference }) {
    const fieldModels = useContext(fieldModelContext)[contentReference.contentTypeId];

    const ref = createRef(null);

    useEffect(() => {
        const instance = ref.current;

        const callback = (event) => {
            stateManager.registerChange(contentReference, event.detail.change);
        };

        instance.addEventListener('cloudy-ui-form-change', callback);

        return () => {
            instance.removeEventListener('cloudy-ui-form-change', callback);
        };
    }, [contentReference]);

    const state = useContext(stateContext);

    const [initialState, setInitialState] = useState();

    useEffect(() => {
        setInitialState(state);
    }, [state.contentReference, state.loading, state.referenceValues, state.loadingNewVersion, state.changedFields]);

    return html`
        <div class='cloudy-ui-form ${state.loading || state.loadingNewVersion ? 'cloudy-ui-loading' : null}' ref=${ref}>
            ${fieldModels.map(fieldModel => renderField(fieldModel, initialState, [fieldModel.descriptor.id]))}
        <//>
    `;
}

export default Form;
