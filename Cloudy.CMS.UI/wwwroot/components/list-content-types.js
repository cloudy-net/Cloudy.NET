import { useState, useEffect, useContext } from '../lib/preact.hooks.module.js';
import html from '../html.js';
import ContentTypeProvider from '../ContentAppSupport/utils/content-type-provider.js';
import List from '../ListSupport/list.js';
import ListItem from '../ListSupport/list-item.js';
import listContentTypeContext from '../list-content-type-context.js';

function ListContentTypes(props) {
    const [listContentType, setListContentType] = useContext(listContentTypeContext);
    const [items, setItems] = useState([]);

    useEffect(() => {
        ContentTypeProvider.getAll().then(items => setItems(items));
    }, []);

    return html`
        <cloudy-ui-blade>
            <cloudy-ui-blade-title><cloudy-ui-blade-title-text>What to edit<//><//>
            <cloudy-ui-blade-content>
                <${List}>
				    ${items.map(item => html`<${ListItem} active=${item == listContentType} text=${item.name} onclick=${() => setListContentType(item)} />`)}
                <//>
            <//>
        <//>
    `;
}

export default ListContentTypes;
