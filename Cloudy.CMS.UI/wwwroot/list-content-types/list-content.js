import { useState, useEffect, useContext } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import List from '../components/list/list.js';
import ListItem from '../components/list/list-item.js';
import urlFetcher from '../util/url-fetcher.js';
import nameGetter from '../data/name-getter.js';
import listContentTypeContext from '../list-content-types/list-content-type-context.js';
import editContentContext from '../edit-content/edit-content-context.js';
import Button from '../components/button/button.js';
import Blade from '../components/blade/blade.js';

function ListContent() {
    const [contentType, setContentType] = useContext(listContentTypeContext);
    const [editingContent, setEditingContent] = useContext(editContentContext);

    if (!contentType) {
        return null;
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
                //setEditingContent({ keys: item.Items[0].Keys, contentTypeId: item.Items[0].ContentTypeId });
            });
    }, []);

    const toolbar = html`<${Button} text="New"><//>`;

    return html`
        <${Blade} title=${contentType.pluralName} toolbar=${toolbar} onclose=${() => setContentType(null)}>
            <cloudy-ui-blade-content>
                <${List}>
			        ${items.map(item => html`
                        <${ListItem}
                            active=${item?.Keys[0] == editingContent?.keys[0]}
                            text=${nameGetter.getNameOf(item, contentType)}
                            onclick=${() => setEditingContent({ keys: item.Keys, contentTypeId: item.ContentTypeId })}
                        />
                    `)}
                <//>
            <//>
        <//>
    `;
}

export default ListContent;
