import { useState, useContext, useEffect } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import listContentTypeContext from '../list-content-types/list-content-type-context.js';
import editContentReferenceContext from './edit-content-reference-context.js';
import nameGetter from '../data/name-getter.js';
import Urls from './urls.js';
import Form from './form.js';
import createEditingContentState from './create-editing-content-state.js';

function EditContent() {
    const [contentType] = useContext(listContentTypeContext);
    const [editingContentReference, setEditingContentReference] = useContext(editContentReferenceContext);

    if (!contentType || !editingContentReference) {
        return null;
    }

    const [editingContentState, setEditingContentState] = useState();

    useEffect(() => {
        if (editingContentReference.newContentKey) {
            setEditingContentState(createEditingContentState(editingContentReference));
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
                    <${Urls}/>
                <//>
                <cloudy-ui-blade-close onclick=${() => setEditingContent(null)}><//>
            <//>
            <cloudy-ui-blade-content>
                <${Form} editingContentState=${editingContentState}/>
            <//>
            <cloudy-ui-blade-footer style="">
                <cloudy-ui-button disabled=${!hasChanges} style="margin-left: auto;" onclick=${() => reviewChanges()}>Review changes</cloudy-ui-button>
                <cloudy-ui-button class="primary" disabled=${!hasChanges} style="margin-left: 10px;" onclick=${() => saveNow()}>Save now</cloudy-ui-button>
            </cloudy-ui-blade-footer>
        <//>
    `;
}

export default EditContent;
