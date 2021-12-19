import html from '../util/html.js';
import { useContext } from '../lib/preact.hooks.module.js';
import showDiffContext from './show-diff-context.js';
import Blade from '../components/blade/blade.js';

function ShowDiff() {
    const [showingDiff] = useContext(showDiffContext);

    if (!showingDiff) {
        return null;
    }

    return html`
        <${Blade} title=${'Pending changes' + (showingDiff.changedFields.length ? ` (${showingDiff.changedFields.length})` : '')}/>
    `;
}

export default ShowDiff;