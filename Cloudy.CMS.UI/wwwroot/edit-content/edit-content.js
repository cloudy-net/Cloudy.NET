import { useState, useEffect, useContext } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import listContentTypeContext from '../list-content-types/list-content-type-context.js';
import editContentContext from '../edit-content/edit-content-context.js';
import contentGetter from '../data/content-getter.js';
import nameGetter from '../data/name-getter.js';
import Urls from './urls.js';
import Form from './form.js';

function EditContent() {
    const [contentType] = useContext(listContentTypeContext);
    const [editingContent, editContent] = useContext(editContentContext);

    if (!contentType || !editingContent) {
        return null;
    }

    const [content, setContent] = useState();

    useEffect(() => {
        editingContent.keys && contentGetter.get(editingContent.keys, editingContent.contentTypeId).then(content => setContent(content));
    }, null);

    if (!content) {
        return null;
    }

    return html`
        <cloudy-ui-blade>
            <cloudy-ui-blade-title>
                <cloudy-ui-blade-title-text>${(
                    editingContent.keys ?
                    `Edit ${nameGetter.getNameOf(content, contentType)}`:
                    `New ${contentType.name}`
                )}<//>
                <cloudy-ui-blade-toolbar>
                    <${Urls}/>
                <//>
                <cloudy-ui-blade-close onclick=${() => editContent(null)}><//>
            <//>
            <cloudy-ui-blade-content>
                <${Form} content=${content}/>
            <//>
        <//>
    `;
}

export default EditContent;
