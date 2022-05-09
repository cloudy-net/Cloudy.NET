import { useContext, useEffect } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import nameGetter from '../data/name-getter.js';
import Urls from './urls.js';
import Form from './form.js';
import contentTypeProvider from '../data/content-type-provider.js';
import stateContext from './state-context.js';
import Blade from '../components/blade/blade.js';

function EditContent({ contentReference, onClose, canDiff, onDiff }) {
    if (!contentReference) {
        return;
    }

    const contentType = contentTypeProvider.get(contentReference.contentTypeId);

    const state = useContext(stateContext);

    var hasChanges = state.changedFields?.length > 0;

    const getTitle = () => {
        const state = useContext(stateContext);
        const getBadge = () => html`<cloudy-ui-change-badge class=${state.changedFields.length ? 'cloudy-ui-unchanged' : null} title="This ${contentType.lowerCaseName} has pending changes."><//>`;

        return state.loading ?
            `Edit ${state.nameHint}` :
            contentReference.keyValues ?
                html`Edit ${nameGetter.getNameOf(state.referenceValues, contentType)}${getBadge()}` :
                `New ${contentType.name}`;
    };

    const toolbar = html`<${Urls} contentReference=${contentReference}/>`;
    const diffButton = canDiff ? html`<cloudy-ui-button disabled=${!hasChanges} onclick=${() => onDiff()}>${hasChanges ? 'Review' : 'No'} changes</cloudy-ui-button>` : null

    return html`
        <${Blade} scrollIntoView=${contentReference} title=${getTitle()} toolbar=${toolbar} onClose=${() => onClose()}>
            <cloudy-ui-blade-content>
                <${Form} contentReference=${contentReference}/>
            <//>
            <cloudy-ui-blade-footer>
                ${hasChanges ? html`<cloudy-ui-blade-footer-note>Draft saved locally.<//>` : null}
                ${diffButton}
            </cloudy-ui-blade-footer>
        <//>
    `;
}

export default EditContent;
