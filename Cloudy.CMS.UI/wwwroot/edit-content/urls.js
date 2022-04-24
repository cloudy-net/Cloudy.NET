import { useState } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import urlFetcher from '../util/url-fetcher.js';
import ListItem from '../components/list/list-item.js';
import PopupMenu from '../components/popup-menu/popup-menu.js';

function Urls({ contentReference }) {
    const [urls, setUrls] = useState();

    urlFetcher.fetch(
        `GetUrl/GetUrl`,
        {
            credentials: 'include',
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                keyValues: contentReference.keys,
                contentTypeId: contentReference.contentTypeId
            })
        },
        'Could not get URL'
    )
        .then(urls => setUrls(urls));

    return html`
        <${PopupMenu} text='View'>
            ${urls && urls.map(url => html`<${ListItem} text=${url}/>`)}
        <//>
    `;
}

export default Urls;
