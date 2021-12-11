import { useState, useEffect, useContext } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import listContentTypeContext from '../list-content-types/list-content-type-context.js';
import editContentContext from '../edit-content/edit-content-context.js';
import contentGetter from './content-getter.js';
import nameGetter from './name-getter.js';
import urlFetcher from '../util/url-fetcher.js';
import ListItem from '../components/list/list-item.js';
import PopupMenu from '../components/popup-menu/popup-menu.js';

function EditContent() {
    const [contentType] = useContext(listContentTypeContext);
    const [editingContent, editContent] = useContext(editContentContext);

    if (!contentType || !editingContent) {
        return null;
    }

    const [content, setContent] = useState();

    useEffect(() => {
        editingContent.keys && contentGetter.get(editingContent.keys, contentType.id).then(content => setContent(content));
    }, null);

    if (!content) {
        return null;
    }

    const [urls, setUrls] = useState();

    useEffect(() => {
        if (!editingContent.keys || !contentType.isRoutable) {
            return;
        }

        urlFetcher.fetch(
            `GetUrl/GetUrl`,
            {
                credentials: 'include',
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    keyValues: editingContent.keys,
                    contentTypeId: contentType.id
                })
            },
            'Could not get URL'
        )
            .then(urls => setUrls(urls));
    }, [editingContent.keys]);

    return html`
        <cloudy-ui-blade>
            <cloudy-ui-blade-title>
                <cloudy-ui-blade-title-text>${(
                    editingContent.keys ?
                    `Edit ${nameGetter.getNameOf(content, contentType)}`:
                    `New ${contentType.name}`
                )}<//>
                <cloudy-ui-blade-close onclick=${() => editContent(null)}><//>
            <//>
            <cloudy-ui-blade-toolbar>
                <${PopupMenu}>
                    <${ListItem}/>
                <//>
            <//>
            <cloudy-ui-blade-content>

            <//>
        <//>
    `;
}

export default EditContent;
