import html from '../util/html.js';
import Blade from '../components/blade/blade.js';
import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';
import ListItem from '../components/list/list-item.js';
import contentGetter from '../data/content-getter.js';
import contentTypeGetter from '../data/content-type-getter.js';
import nameGetter from '../data/name-getter.js';
import showDiffContext from './show-diff-context.js';
import pendingChangesContext from './pending-changes-context.js';

function PendingChanges() {
    const [pendingChanges] = useContext(pendingChangesContext);
    const [items, setItems] = useState([]);
    const [, showDiffBlade, setDiffData, setShowDiffBlade] = useContext(showDiffContext);

    useEffect(() => {
        if (pendingChanges && pendingChanges.length) {
            Promise.all(pendingChanges.map(async (change) => {
                const contentType = await contentTypeGetter.get(change.contentTypeId);
                const content = await contentGetter.get([change.contentId], change.contentTypeId);
                return { change, name: nameGetter.getNameOf(content, contentType), contentType };
            })).then(items => {
                setItems(items);
            })
        }
    }, [pendingChanges])

    if (!showDiffBlade) {
        return;
    }

    return html`
        <${Blade} title='Pending changes' onclose=${() => setShowDiffBlade(false)}>
            ${items && !items.length ? 'No more pending changes' : null}
            ${items && items.length ? items.map(item => html`
                <${ListItem}
                    text=${item.name}
                    subtext=${item.change.remove ? 'Slated for removal' : item.change.contentId ? `Changed fields: ${item.change.changedFields.length}` : `New ${item.contentType.lowerCaseName}`}
                    onclick=${() => setDiffData(item.change)}
                />`) : null}
        <//>
    `;
}

export default PendingChanges;