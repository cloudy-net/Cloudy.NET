import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import editContentContext from '../edit-content/edit-content-context.js';
import pendingChangesContext from '../diff/pending-changes-context.js';
import fieldModelBuilder from '../FormSupport/field-model-builder.js';
import FormField from './form-field.js';
import { createRef } from '../lib/preact.module.js';

function Form(props) {
    const [editingContent] = useContext(editContentContext);
    const [pendingChanges, updatePendingChanges, , getPendingValue] = useContext(pendingChangesContext);

    if (!editingContent) {
        return null;
    }

    const [fieldModels, setFieldModels] = useState();

    useEffect(() => {
        fieldModelBuilder.getFieldModels(editingContent.contentTypeId)
            .then(fieldModels => setFieldModels(fieldModels));
    }, [editingContent.contentTypeId]);

    if (!fieldModels) {
        return null;
    }

    const ref = createRef(null);

    useEffect(() => {
        ref.current.addEventListener('cloudy-ui-form-change', (event) => {
            updatePendingChanges({
                keys: editingContent.keys,
                contentTypeId: editingContent.contentTypeId,
                change: event.detail.change,
            });
        });
    });

    return html`
        <div class='cloudy-ui-form' ref=${ref}>
            ${fieldModels.map(fieldModel => html`
            <${FormField}
                contentId=${editingContent.keys}
                contentTypeId=${editingContent.contentTypeId}
                initialValue=${getPendingValue(editingContent.keys, editingContent.contentTypeId, [fieldModel.descriptor.id], props.content[fieldModel.descriptor.id])}
                fieldModel=${fieldModel}
            />`)}
        <//>
    `;
}

export default Form;
