import { useState, useEffect, useContext } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import List from '../components/list/list.js';
import ListItem from '../components/list/list-item.js';
import urlFetcher from '../util/url-fetcher.js';
import nameGetter from '../data/name-getter.js';
import Button from '../components/button/button.js';
import Blade from '../components/blade/blade.js';
import listContentContext from './list-content-context.js';
import arrayEquals from '../util/array-equals.js';

function ListContent({ contentType, activeContentReference, onEditContent, onNewContent, onClose }) {
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

    const states = useContext(listContentContext);

    const getBadges = item => {
        const state = states.find(s => arrayEquals(s.contentReference.keyValues, item.Keys));

        if(!state || !state.changedFields?.length){
            return;
        }

        return html`<cloudy-ui-list-item-badge class="cloudy-ui-modified" title="${state.changedFields.length} changed fields"><//>`;
    };

    return html`
        <${Blade} title=${contentType.pluralName} toolbar=${toolbar} onClose=${() => onClose()}>
            <cloudy-ui-blade-content>
                <${List}>
			        ${items.map(item => html`
                        <${ListItem}
                            active=${!activeContentReference?.newContentKey && item.Keys[0] == activeContentReference?.keyValues[0]}
                            text=${nameGetter.getNameOf(item, contentType)}
                            badges=${getBadges(item)}
                            onclick=${() => onEditContent({ keyValues: item.Keys, contentTypeId: item.ContentTypeId }, nameGetter.getNameOf(item, contentType))}
                        />
                    `)}
                <//>
            <//>
        <//>
    `;
}

export default ListContent;
