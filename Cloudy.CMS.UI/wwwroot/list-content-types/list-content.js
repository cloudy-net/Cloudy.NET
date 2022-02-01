﻿import { useState, useEffect, useContext, useCallback } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import List from '../components/list/list.js';
import ListItem from '../components/list/list-item.js';
import urlFetcher from '../util/url-fetcher.js';
import nameGetter from '../data/name-getter.js';
import listContentTypeContext from '../list-content-types/list-content-type-context.js';
import editContentContext from '../edit-content/edit-content-context.js';

function ListContent() {
    const [contentType, setContentType, singleton] = useContext(listContentTypeContext);
    const [editingContent, setEditingContent] = useContext(editContentContext);

    if (!contentType) {
        return null;
    }

    useEffect(() => {

    }, [])

    const [items, setItems] = useState([]);

    useEffect(() => {
        urlFetcher.fetch(
            'ContentList/Get?contentTypeIds[0]=' + contentType.id,
            { credentials: 'include' },
            'Could not load content list'
        )
            .then(item => {
                setItems(item.Items);
                setEditingContent({ keys: item.Items[0].Keys, contentTypeId: item.Items[0].ContentTypeId });
            });
    }, []);


    return html`
        <cloudy-ui-blade>
            <cloudy-ui-blade-title>
                <cloudy-ui-blade-title-text>${contentType.pluralName}<//>
                ${!singleton ? html`<cloudy-ui-blade-close onclick=${() => setContentType(null)}><//>` : null}
            <//>
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
