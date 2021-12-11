import { useState, useEffect, useContext } from '../lib/preact.hooks.module.js';
import html from '../html.js';
import List from '../ListSupport/list.js';
import listContentTypeContext from '../list-content-types/list-content-type-context.js';
import editContentContext from '../edit-content/edit-content-context.js';

function EditContent() {
    const [contentType] = useContext(listContentTypeContext);
    const [editingContent, editContent] = useContext(editContentContext);

    if (!contentType || !editingContent) {
        return null;
    }

    useEffect(() => {
        
    }, null);

    return html`
        <cloudy-ui-blade>
            <cloudy-ui-blade-title>
                <cloudy-ui-blade-title-text>Edit ${'...'}<//>
                <cloudy-ui-blade-close onclick=${() => editContent(null)}><//>
            <//>
            <cloudy-ui-blade-content>

            <//>
        <//>
    `;
}

export default EditContent;
