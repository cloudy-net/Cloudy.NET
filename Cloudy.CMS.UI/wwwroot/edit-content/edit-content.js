import { useState, useEffect, useContext, useCallback } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import listContentTypeContext from '../list-content-types/list-content-type-context.js';
import editContentContext from '../edit-content/edit-content-context.js';
import pendingChangesContext from '../diff/pending-changes-context.js';
import showDiffContext from '../diff/show-diff-context.js';
import contentGetter from '../data/content-getter.js';
import nameGetter from '../data/name-getter.js';
import Urls from './urls.js';
import Form from './form.js';

function EditContent() {
    const [contentType] = useContext(listContentTypeContext);
    const [editingContent, setEditingContent] = useContext(editContentContext);
    const [, , setDiffData] = useContext(showDiffContext);
    const [pendingChanges, , , , getFor, applyFor] = useContext(pendingChangesContext);
    const [hasChanges, setHasChanges] = useState(false);

    if (!contentType || !editingContent) {
        return null;
    }

    const [content, setContent] = useState();

    useEffect(() => {
        editingContent.keys && contentGetter.get(editingContent.keys, editingContent.contentTypeId).then(content => {
            setContent(content);
        });
    }, [editingContent]);

    useEffect(() => {
        setHasChanges(!!getFor(editingContent.keys[0], editingContent.contentTypeId));
    }, [editingContent, pendingChanges]);

    const reviewChanges = useCallback(() => {
        setDiffData(getFor(editingContent.keys[0], editingContent.contentTypeId))
    }, [editingContent, getFor]);

    const saveNow = useCallback(() => {
        applyFor(editingContent.keys[0], editingContent.contentTypeId, () => {
            setDiffData(null);
        });
    }, [applyFor]);

    return content ? html`
        <cloudy-ui-blade>
            <cloudy-ui-blade-title>
                <cloudy-ui-blade-title-text>${(
            editingContent.keys ?
                `Edit ${nameGetter.getNameOf(content, contentType)}` :
                `New ${contentType.name}`
        )}<//>
                <cloudy-ui-blade-toolbar>
                    <${Urls}/>
                <//>
                <cloudy-ui-blade-close onclick=${() => setEditingContent(null)}><//>
            <//>
            <cloudy-ui-blade-content>
                <${Form} content=${content}/>
            <//>
            <cloudy-ui-blade-footer style="">
                <cloudy-ui-button disabled=${!hasChanges} style="margin-left: auto;" onclick=${() => reviewChanges()}>Review changes</cloudy-ui-button>
                <cloudy-ui-button class="primary" disabled=${!hasChanges} style="margin-left: 10px;" onclick=${() => saveNow()}>Save now</cloudy-ui-button>
            </cloudy-ui-blade-footer>
        <//>
    `: null;
}

export default EditContent;
