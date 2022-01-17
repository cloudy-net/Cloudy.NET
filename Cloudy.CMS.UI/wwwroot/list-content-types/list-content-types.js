import { useState, useEffect, useContext } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import ContentTypeProvider from '../data/content-type-getter.js';
import List from '../components/list/list.js';
import ListItem from '../components/list/list-item.js';
import listContentTypeContext from '../list-content-types/list-content-type-context.js';

function ListContentTypes() {
    const [listContentType, setListContentType, , setSingleton] = useContext(listContentTypeContext);
    const [items, setItems] = useState([]);

    useEffect(() => {
        ContentTypeProvider.getAll().then(items => {
            if (items?.length === 1) {
                setListContentType(items[0]);
                setSingleton(true);
            } else {
                setItems(items);
                setSingleton(false);
            }
        });
    }, []);

    if (listContentType) {
        return null;
    }

    return html`
        <cloudy-ui-blade>
            <cloudy-ui-blade-title><cloudy-ui-blade-title-text>What to edit<//><//>
            <cloudy-ui-blade-content>
                <${List}>
				    ${items.map(item => html`
                        <${ListItem}
                            active=${item == listContentType}
                            text=${item.name}
                            onclick=${() => setListContentType(item)}
                        />
                    `)}
                <//>
            <//>
        <//>
    `;
}

export default ListContentTypes;
