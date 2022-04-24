import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import editContentReferenceContext from './edit-content-reference-context.js';
import pendingChangesContext from '../diff/pending-changes-context.js';
import fieldModelBuilder from '../FormSupport/field-model-builder.js';
import FormField from './form-field.js';
import { createRef } from '../lib/preact.module.js';
import contentStateManager from './content-state-manager.js';

function Form({ contentReference }) {
    const [editingContentReference] = useContext(editContentReferenceContext);
    const [pendingChanges, updatePendingChanges, , getPendingValue] = useContext(pendingChangesContext);

    if (!editingContentReference) {
        return null;
    }

    const [fieldModels, setFieldModels] = useState();

    useEffect(() => {
        fieldModelBuilder.getFieldModels(editingContentReference.contentTypeId)
            .then(fieldModels => setFieldModels(fieldModels));
    }, [editingContentReference.contentTypeId]);

    if (!fieldModels) {
        return null;
    }

    const ref = createRef(null);

    useEffect(() => {
        const callback = (event) => {
            contentStateManager.registerChange({
                ...editingContentReference,
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
                contentId=${editingContentReference.keys}
                contentTypeId=${editingContentReference.contentTypeId}
                initialValue=${editingContentState.values[fieldModel.descriptor.id]}
                fieldModel=${fieldModel}
            />`)}
        <//>
    `;
}

export default Form;
