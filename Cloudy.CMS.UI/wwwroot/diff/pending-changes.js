import html from '../util/html.js';
import Blade from '../components/blade/blade.js';
import ListItem from '../components/list/list-item.js';
import stateManager from '../edit-content/state-manager.js';
import contentTypeProvider from '../data/content-type-provider.js';
import nameGetter from '../data/name-getter.js';
import { useEffect, useState } from '../lib/preact.hooks.module.js';
import diff from './lib/diff.js';

function PendingChanges({ renderIf, onSelect, onClose }) {
    if (!renderIf) {
        return;
    }

    const [states, setStates] = useState(stateManager.getAll());

    useEffect(() => {
        const callback = () => setStates(stateManager.getAll());
        stateManager.onAnyStateChange(callback);

        return () => { stateManager.offAnyStateChange(callback); };
    }, []);

    const groups = contentTypeProvider
        .getAll()
        .map(c => ({ contentType: c, changes: states.filter(s => s.contentReference.contentTypeId == c.id) }))
        .filter(g => g.changes.length);

    const getSubText = (state, contentType) => state.remove ? 'Slated for removal' : state.contentReference.keyValues ? `Changed fields: ${state.changedFields.length}` : `New ${contentType.lowerCaseName} (created ${state.referenceDate.toLocaleString()})`;
    const getName = (state, contentType) => state.contentReference.newContentKey ? nameGetter.getNameOfState(state, contentType) : diff(nameGetter.getNameOf(state.referenceValues, contentType) || '', nameGetter.getNameOfState(state, contentType) || '', 0).map(([state, segment]) => html`<span class=${state == diff.INSERT ? 'cloudy-ui-diff-insert' : state == diff.DELETE ? 'cloudy-ui-diff-delete' : null}>${segment}</span>`);
    const renderItems = (states, contentType) => states.map(state => html`<${ListItem} text=${getName(state, contentType)} subtext=${getSubText(state, contentType)} onclick=${() => onSelect(state.contentReference)} />`)

    return html`
        <${Blade} title='Pending changes' onClose=${() => onClose()}>
            <cloudy-ui-blade-content>
                ${!groups.length ? html`<cloudy-ui-list-sub-header>No more pending changes<//>` : null}
                ${groups.length ? groups.map(group => html`${groups.length > 1 ?
                    html`<cloudy-ui-list-sub-header>${group.contentType.name}<//>` : null}${renderItems(group.changes, group.contentType)}`) : null}
            <//>
            <cloudy-ui-blade-footer>
                <cloudy-ui-button tabindex="0" class="primary" disabled=${!groups.length} onclick=${() => saveAll()}>Save all</cloudy-ui-button>
            </cloudy-ui-blade-footer>
        <//>
    `;
}

export default PendingChanges;