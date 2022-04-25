import { useEffect, useState } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import fieldModelBuilder from '../FormSupport/field-model-builder.js';
import FormField from './form-field.js';
import { createRef } from '../lib/preact.module.js';
import contentStateManager from './content-state-manager.js';

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
        const callback = (event) => {
            contentStateManager.registerChange({
                ...contentReference,
                change: event.detail.change,
            });
        };

        ref.current.addEventListener('cloudy-ui-form-change', callback);

        return () => {
            ref.current.removeEventListener('cloudy-ui-form-change', callback);
        };
    });

    return html`
        <div class='cloudy-ui-form' ref=${ref}>
            ${fieldModels.map(fieldModel => html`
            <${FormField}
                contentId=${contentReference.keys}
                contentTypeId=${contentReference.contentTypeId}
                initialValue=${editingContentState.values[fieldModel.descriptor.id]}
                fieldModel=${fieldModel}
            />`)}
        <//>
    `;
}

export default Form;
