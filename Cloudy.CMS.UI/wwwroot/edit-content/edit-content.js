import { useState, useContext, useEffect } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import contentTypesContext from '../list-content-types/content-types-context.js';
import editContentReferenceContext from './edit-content-reference-context.js';
import nameGetter from '../data/name-getter.js';
import Urls from './urls.js';
import Form from './form.js';
import contentStateManager from './content-state-manager.js';

function EditContent({ contentReference }) {
    const [contentTypes] = useContext(contentTypesContext);
    let contentType;

    useEffect(() => {
        if (!contentTypes) {
            return;
        }

        contentType = contentTypes.find(t => t.id == contentReference.contentTypeId);
    }, [contentReference, contentType]);

    if (!contentType) {
        return;
    }

    const [editingContentReference, setEditingContentReference] = useContext(editContentReferenceContext);

    if (!contentType || !editingContentReference) {
        return null;
    }

    const [editingContentState, setEditingContentState] = useState();

    useEffect(() => {
        if (editingContentReference.newContentKey) {
            setEditingContentState(contentStateManager.createEditingContentState(editingContentReference));
        }
    }, [editingContentReference]);

    var hasChanges = false;

    return html`
        <cloudy-ui-blade>
            <cloudy-ui-blade-title>
                <cloudy-ui-blade-title-text>${(
            editingContentReference.keys ?
                `Edit ${nameGetter.getNameOf(content, contentType)}` :
                `New ${contentType.name}`
        )}<//>
                <cloudy-ui-blade-toolbar>
                    <${Urls} contentReference=${contentReference}/>
                <//>
                <cloudy-ui-blade-close onclick=${() => setEditingContentReference(null)}><//>
            <//>
            <cloudy-ui-blade-content>
                <${Form}/>
            <//>
            <cloudy-ui-blade-footer style="">
                <cloudy-ui-button disabled=${!hasChanges} style="margin-left: auto;" onclick=${() => reviewChanges()}>Review changes</cloudy-ui-button>
                <cloudy-ui-button class="primary" disabled=${!hasChanges} style="margin-left: 10px;" onclick=${() => saveNow()}>Save now</cloudy-ui-button>
            </cloudy-ui-blade-footer>
        <//>
    `;
}

export default EditContent;
