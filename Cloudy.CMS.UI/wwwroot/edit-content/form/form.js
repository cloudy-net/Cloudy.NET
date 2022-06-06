import { useContext, useEffect, useState } from '../../lib/preact.hooks.module.js';
import html from '../../util/html.js';
import stateContext from '../state-context.js';
import fieldDescriptorProvider from './field-descriptor-provider.js';
import renderField from './render-field.js';

function Form({ contentReference }) {
    const fieldDescriptors = fieldDescriptorProvider.get(contentReference.contentTypeId);

    const state = useContext(stateContext);

    const [initialState, setInitialState] = useState();

    useEffect(() => {
        setInitialState(state);
    }, [state.contentReference, state.loading, state.referenceValues, state.loadingNewVersion, state.simpleChanges]);

    return html`
        <div class='cloudy-ui-form ${state.loading || state.loadingNewVersion ? 'cloudy-ui-loading' : null}'>
            ${fieldDescriptors.map(fieldDescriptor => renderField(fieldDescriptor, initialState, [fieldDescriptor.id]))}
        <//>
    `;
}

export default Form;
