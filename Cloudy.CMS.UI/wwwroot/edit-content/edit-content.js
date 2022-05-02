import { useContext } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import nameGetter from '../data/name-getter.js';
import Urls from './urls.js';
import Form from './form.js';
import contentTypeProvider from '../data/content-type-provider.js';
import stateContext from './state-context.js';
import Blade from '../components/blade/blade.js';

function EditContent({ contentReference, onClose, onReviewChanges }) {
    if (!contentReference) {
        return;
    }

    const contentType = contentTypeProvider.get(contentReference.contentTypeId);

    const state = useContext(stateContext);

    var hasChanges = state.changedFields?.length > 0;

    const getTitle = () =>
        state.loading ?
            `Edit ${state.nameHint}` :
            contentReference.keyValues ?
                `Edit ${nameGetter.getNameOfState(state, contentType)}` :
                `New ${contentType.name} ${state.changedFields.length}`;

    const toolbar = html`<${Urls} contentReference=${contentReference}/>`;

    return html`
        <${Blade} title=${getTitle()} toolbar=${toolbar} onClose=${() => onClose()}>
            <cloudy-ui-blade-content>
                <${Form} contentReference=${contentReference}/>
            <//>
            <cloudy-ui-blade-footer>
                <cloudy-ui-button disabled=${!hasChanges} style="margin-left: auto;" onclick=${() => onReviewChanges()}>Review changes</cloudy-ui-button>
                <cloudy-ui-button class="primary" disabled=${!hasChanges} style="margin-left: 10px;" onclick=${() => saveNow()}>Save now</cloudy-ui-button>
            </cloudy-ui-blade-footer>
        <//>
    `;
}

export default EditContent;
