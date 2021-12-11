import { useState, useEffect, useContext } from '../lib/preact.hooks.module.js';
import html from '../html.js';
import List from '../ListSupport/list.js';
import ListItem from '../ListSupport/list-item.js';
import urlFetcher from '../url-fetcher.js';
import nameGetter from '../edit-content/name-getter.js';
import listContentTypeContext from '../list-content-types/list-content-type-context.js';
import editContentContext from '../edit-content/edit-content-context.js';

function ListContent() {
    const [contentType, setContentType] = useContext(listContentTypeContext);
    const [editingContent, editContent] = useContext(editContentContext);

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
            .then(items => setItems(items.Items));
    }, []);

    return html`
        <cloudy-ui-blade>
            <cloudy-ui-blade-title>
                <cloudy-ui-blade-title-text>${contentType.pluralName}<//>
                <cloudy-ui-blade-close onclick=${() => setContentType(null)}><//>
            <//>
            <cloudy-ui-blade-content>
                <${List}>
			        ${items.map(item => html`
                        <${ListItem}
                            active=${item == editingContent}
                            text=${nameGetter.getNameOf(item, contentType)}
                            onclick=${() => editContent(item)}
                        />
                    `)}
                <//>
            <//>
        <//>
    `;
}

export default ListContent;
