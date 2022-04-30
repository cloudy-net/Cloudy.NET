import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import fieldModelBuilder from '../FormSupport/field-model-builder.js';
import FormField from './form-field.js';
import { createRef } from '../lib/preact.module.js';
import stateManager from './state-manager.js';
import stateContext from './state-context.js';

function Form({ contentReference }) {
    const [fieldModels, setFieldModels] = useState();

    useEffect(() => {
        fieldModelBuilder.getFieldModels(contentReference.contentTypeId)
            .then(fieldModels => setFieldModels(fieldModels));
    }, [contentReference.contentTypeId]);

    if (!fieldModels) {
        return null;
    }

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

    return html`
        <div class='cloudy-ui-form ${state.loading ? 'cloudy-ui-loading' : null}' ref=${ref}>
            ${fieldModels.map(fieldModel => html`
            <${FormField}
                fieldModel=${fieldModel}
            />`)}
        <//>
    `;
}

export default Form;
