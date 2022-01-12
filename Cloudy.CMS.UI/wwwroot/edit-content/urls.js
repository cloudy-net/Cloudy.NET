import { useState, useEffect, useContext } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import listContentTypeContext from '../list-content-types/list-content-type-context.js';
import editContentContext from '../edit-content/edit-content-context.js';
import urlFetcher from '../util/url-fetcher.js';
import ListItem from '../components/list/list-item.js';
import PopupMenu from '../components/popup-menu/popup-menu.js';

function Urls() {
    const [contentType] = useContext(listContentTypeContext);
    const [editingContent] = useContext(editContentContext);

    if (!contentType || !editingContent) {
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
        <${PopupMenu} text='View'>
            ${urls && urls.map(url => html`<${ListItem} text=${url}/>`)}
        <//>
    `;
}

export default Urls;
