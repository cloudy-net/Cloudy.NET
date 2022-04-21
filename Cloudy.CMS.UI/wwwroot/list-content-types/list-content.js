import { useState, useEffect, useContext } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import List from '../components/list/list.js';
import ListItem from '../components/list/list-item.js';
import urlFetcher from '../util/url-fetcher.js';
import nameGetter from '../data/name-getter.js';
import listContentTypeContext from '../list-content-types/list-content-type-context.js';
import editContentReferenceContext from '../edit-content/edit-content-reference-context.js';
import Button from '../components/button/button.js';
import Blade from '../components/blade/blade.js';
import createEditingContentReference from '../edit-content/create-editing-content-reference.js';

function ListContent() {
    const [contentType, setContentType] = useContext(listContentTypeContext);
    const [editingContentReference, setEditingContentReference] = useContext(editContentReferenceContext);

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
            });
    }, []);

    const newContent = () => {
        setEditingContentReference(createEditingContentReference(contentType));
    };
    const toolbar = html`<${Button} text="New" onclick=${() => newContent()}><//>`;

    return html`
        <${Blade} title=${contentType.pluralName} toolbar=${toolbar} onclose=${() => setContentType(null)}>
            <cloudy-ui-blade-content>
                <${List}>
			        ${items.map(item => html`
                        <${ListItem}
                            active=${item?.Keys[0] == editingContentReference?.keys[0]}
                            text=${nameGetter.getNameOf(item, contentType)}
                            onclick=${() => setEditingContentReference({ keys: item.Keys, contentTypeId: item.ContentTypeId })}
                        />
                    `)}
                <//>
            <//>
        <//>
    `;
}

export default ListContent;
