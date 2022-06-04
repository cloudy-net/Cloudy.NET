import { useEffect, useState } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import urlFetcher from '../util/url-fetcher.js';
import ListItem from '../components/list/list-item.js';
import PopupMenu from '../components/popup-menu/popup-menu.js';

function Urls({ contentReference }) {
    const [urls, setUrls] = useState();

    useEffect(() => {
        urlFetcher.fetch(
            `GetUrl/GetUrl`,
            {
                credentials: 'include',
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(contentReference)
            },
            'Could not get URL'
        )
            .then(urls => setUrls(urls));
    }, [contentReference]);

    return html`
        <${PopupMenu} text='View'>
            <cloudy-ui-info-message style="width: 300px; max-width: 100vw; box-sizing: border-box;">These URLs are not synced with your local changes.<//>
            ${urls && urls.map(url => html`<${ListItem} link="/${url}" target="blank" text=${url}/>`)}
        <//>
    `;
}

export default Urls;
