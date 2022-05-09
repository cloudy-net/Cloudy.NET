import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import fieldModelBuilder from '../FormSupport/field-model-builder.js';
import { createRef } from '../lib/preact.module.js';
import stateManager from './state-manager.js';
import stateContext from './state-context.js';
import SimpleField from './simple-field.js';
import SortableField from './sortable-field.js';

const renderField = (fieldModel, initialState) => {
    if(!initialState || initialState.loading){
        return;
    }

    const path = [fieldModel.descriptor.id];

    if (fieldModel.descriptor.isSortable) {
        return html`<${SortableField}
            path=${path}
            fieldModel=${fieldModel}
            initialState=${initialState}
        />`;
    }

    if (fieldModel.descriptor.embeddedFormId) {
        return html`<${EmbeddedForm}
            path=${path}
            fieldModel=${fieldModel}
            initialState=${initialState}
        />`;
    }

    return html`<${SimpleField}
        path=${path}
        fieldModel=${fieldModel}
        initialState=${initialState}
    />`;
}

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

    const [initialState, setInitialState] = useState();

    useEffect(() => {
        setInitialState(state);
    }, [state.contentReference, state.loading, state.changedFields]);

    return html`
        <div class='cloudy-ui-form ${state.loading ? 'cloudy-ui-loading' : null}' ref=${ref}>
            ${fieldModels.map(fieldModel => renderField(fieldModel, initialState))}
        <//>
    `;
}

export default Form;
