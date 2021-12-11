import { useState, useEffect, useContext } from '../lib/preact.hooks.module.js';
import html from '../html.js';
import listContentTypeContext from '../list-content-types/list-content-type-context.js';
import editContentContext from '../edit-content/edit-content-context.js';
import contentGetter from './content-getter.js';
import nameGetter from './name-getter.js';

function EditContent() {
    const [contentType] = useContext(listContentTypeContext);
    const [editingContent, editContent] = useContext(editContentContext);

    if (!contentType || !editingContent) {
        return null;
    }

    const [content, setContent] = useState();

    useEffect(() => {
        contentGetter.get(editingContent.Keys, contentType.id).then(content => setContent(content));
    }, null);

    if (!content) {
        return null;
    }

    return html`
        <cloudy-ui-blade>
            <cloudy-ui-blade-title>
                <cloudy-ui-blade-title-text>Edit ${nameGetter.getNameOf(content, contentType)}<//>
                <cloudy-ui-blade-close onclick=${() => editContent(null)}><//>
            <//>
            <cloudy-ui-blade-content>

            <//>
        <//>
    `;
}

export default EditContent;
