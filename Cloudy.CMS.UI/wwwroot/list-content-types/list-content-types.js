import html from '../util/html.js';
import contentTypeProvider from '../data/content-type-provider.js';
import List from '../components/list/list.js';
import ListItem from '../components/list/list-item.js';

function ListContentTypes({ renderIf, activeContentType, onSelectContentType }) {
    if (!renderIf) {
        return;
    }

    const items = contentTypeProvider.getAll();

    if (items?.length === 1) {
        onSelectContentType(items[0]);
    }

    if (activeContentType) {
        return;
    }

    return html`
        <cloudy-ui-blade>
            <cloudy-ui-blade-title><cloudy-ui-blade-title-text>What to edit<//><//>
            <cloudy-ui-blade-content>
                <${List}>
				    ${items.map(item => html`
                        <${ListItem}
                            active=${item == activeContentType}
                            text=${item.name}
                            onclick=${() => onSelectContentType(item)}
                        />
                    `)}
                <//>
            <//>
        <//>
    `;
}

export default ListContentTypes;
