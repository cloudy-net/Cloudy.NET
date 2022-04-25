import html from '../util/html.js';
import Blade from '../components/blade/blade.js';
import { useContext, useEffect, useState, useCallback } from '../lib/preact.hooks.module.js';
import ListItem from '../components/list/list-item.js';
import contentGetter from '../data/content-getter.js';
import contentTypeGetter from '../data/content-type-provider.js';
import nameGetter from '../data/name-getter.js';
import showDiffContext from './show-diff-context.js';
import pendingChangesContext from './pending-changes-context.js';

function PendingChanges() {
    const [pendingChanges, , , , , , applyAll] = useContext(pendingChangesContext);
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
        } else {
            setItems([]);
        }
    }, [pendingChanges]);

    if (!showDiffBlade) {
        return;
    }

    const saveChanges = useCallback(() => {
        applyAll(pendingChanges, () => {
            // setShowDiffBlade(false);
        });
    }, [pendingChanges, applyAll]);

    return html`
        <${Blade} title='Pending changes' onclose=${() => setShowDiffBlade(false)}>
            <cloudy-ui-blade-content>
                ${items && !items.length ? html`<cloudy-ui-list-sub-header>No more pending changes<//>` : null}
                ${items && items.length ? items.map(item => html`
                <${ListItem}
                    text=${item.name}
                    subtext=${item.change.remove ? 'Slated for removal' : item.change.contentId ? `Changed fields: ${item.change.changedFields.length}` : `New ${item.contentType.lowerCaseName}`}
                    onclick=${() => setDiffData(item.change)}
                />`) : null}
            <//>
            <cloudy-ui-blade-footer>
                <cloudy-ui-button tabindex="0" class="primary" style="margin-left: auto;" disabled=${!pendingChanges.length} onclick=${() => saveChanges()}>Save</cloudy-ui-button>
            </cloudy-ui-blade-footer>
        <//>
    `;
}

export default PendingChanges;