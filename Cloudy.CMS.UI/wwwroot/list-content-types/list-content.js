import { useState, useEffect } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import List from '../components/list/list.js';
import ListItem from '../components/list/list-item.js';
import urlFetcher from '../util/url-fetcher.js';
import nameGetter from '../data/name-getter.js';
import Button from '../components/button/button.js';
import Blade from '../components/blade/blade.js';

function ListContent({ renderIf, contentType, onEditContent, onNewContent, onClose }) {
    if (!renderIf) {
        return;
    }

    if (!contentType) {
        return;
    }

    const [items, setItems] = useState([]);

    useEffect(() => {
        urlFetcher.fetch(
            'ContentList/Get?contentTypeIds[0]=' + contentType.id,
            { credentials: 'include' },
            'Could not load content list'
        )
            .then(item => {
                setItems(item.Items);
            });
    }, []);

    const toolbar = html`<${Button} text="New" onClick=${() => onNewContent(contentType)}><//>`;

    return html`
        <${Blade} title=${contentType.pluralName} toolbar=${toolbar} onClose=${() => onClose()}>
            <cloudy-ui-blade-content>
                <${List}>
			        ${items.map(item => html`
                        <${ListItem}
                            active=${item?.Keys[0] == editingContentReference?.keys[0]}
                            text=${nameGetter.getNameOf(item, contentType)}
                            onclick=${() => onEditContent({ keys: item.Keys, contentTypeId: item.ContentTypeId })}
                        />
                    `)}
                <//>
            <//>
        <//>
    `;
}

export default ListContent;
