import { useState, useEffect } from './lib/preact.hooks.module.js';
import html from './html.js';
import List from './ListSupport/list.js';
import ListItem from './ListSupport/list-item.js';
import urlFetcher from './url-fetcher.js';
import nameGetter from './ContentAppSupport/utils/name-getter.js';

function ListContent(props) {
    const contentType = props.contentType;
    const [editingContent, editContent] = useState(null);
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
        <${List}>
			${items.map(item => html`
                <${ListItem}
                    active=${item == editingContent}
                    text=${nameGetter.getNameOf(item, contentType)}
                    onclick=${() => editContent(item)}
                />
            `)}
        <//>
    `;
}

export default ListContent;
