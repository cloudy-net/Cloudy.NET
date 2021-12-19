import html from '../util/html.js';
import Blade from '../components/blade/blade.js';
import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';
import changeTracker from '../edit-content/change-tracker.js';
import ListItem from '../components/list/list-item.js';
import contentGetter from '../data/content-getter.js';
import contentTypeGetter from '../data/content-type-getter.js';
import nameGetter from '../data/name-getter.js';
import showDiffContext from './show-diff-context.js';

function ShowDiff() {
    const [items, setItems] = useState();

    useEffect(() => {
        Promise.all(changeTracker.pendingChanges.map(async change => {
            const contentType = await contentTypeGetter.get(change.contentTypeId);
            const content = await contentGetter.get(change.contentId, change.contentTypeId);

            return { change, name: nameGetter.getNameOf(content, contentType), contentType };
        }))
            .then(items => setItems(items));
    }, []);

    const [showingDiff, showDiff] = useContext(showDiffContext);

    return html`
        <${Blade} title='Pending changes'>
            ${items && !items.length ? 'No more pending changes' : null}
            ${items && items.length ? items.map(item => html`
                <${ListItem}
                    text=${item.name}
                    subtext=${item.change.remove ? 'Slated for removal' : item.change.contentId ? `Changed fields: ${item.change.changedFields.length}` : `New ${item.contentType.lowerCaseName}`}
                    onclick=${() => showDiff(item.change)}
                />`) : null}
        <//>
    `;
}

export default ShowDiff;