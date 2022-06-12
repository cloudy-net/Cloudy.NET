import { useContext } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import nameGetter from '../data/name-getter.js';
import Urls from './urls.js';
import Form from './form/form.js';
import stateContext from './state-context.js';
import Blade from '../components/blade/blade.js';
import contentTypeProvider from '../list-content-types/content-type-provider.js';
import ContextMenu from '../components/context-menu/context-menu.js';
import ListItem from '../components/list/list-item.js';
import Button from '../components/button/button.js';
import stateManager from './state-manager.js';

function EditContent({ contentReference, onClose, canDiff, onDiff, reviewRemoteChanges }) {
    if (!contentReference) {
        return;
    }

    const contentType = contentTypeProvider.get(contentReference.contentTypeId);
    const state = useContext(stateContext);

    var hasChanges = state.simpleChanges?.length > 0;

    const getTitle = () => {
        const state = useContext(stateContext);

        return state.loading && state.nameHint ?
            `Edit ${state.nameHint}` :
            contentReference.keyValues ?
                html`Edit ${nameGetter.getNameOf(state.referenceValues, contentType)}` :
                `New ${contentType.name}`;
    };

    const getConflictMessage = () => state.newVersion ? html`<cloudy-ui-info-message>Conflict detected: This ${contentType.lowerCaseName} has been changed after you started editing. <a onclick=${() => reviewRemoteChanges(true)}>Review remote changes<//> before saving.<//>` : null;

    const toolbar = html`<${Urls} contentReference=${contentReference}/>`;
    const diffButton = canDiff ? html`<${Button} disabled=${!hasChanges} onClick=${() => onDiff()} text="Review changes"/>` : null;

    return html`
        <${Blade} scrollIntoView=${contentReference} title=${getTitle()} toolbar=${toolbar} onClose=${() => onClose()}>
            <cloudy-ui-blade-content>
                ${getConflictMessage()}
                <${Form} contentReference=${contentReference}/>
            <//>
            <cloudy-ui-blade-footer>
                ${hasChanges ? html`<cloudy-ui-blade-footer-note style="margin-right: auto;">Draft saved locally.<//>` : null}
                <${ContextMenu} position="bottom">
                    <${ListItem} disabled=${!hasChanges} text="Discard changes" onclick=${() => stateManager.discardChanges(contentReference)}/>
                <//>
                ${diffButton}
            </cloudy-ui-blade-footer>
        <//>
    `;
}

export default EditContent;
