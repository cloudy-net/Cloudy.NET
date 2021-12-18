import html from '../util/html.js';
import { useContext } from '../lib/preact.hooks.module.js';
import showDiffContext from './show-diff-context.js';
import PendingChanges from './pending-changes.js';

function ShowDiff() {
    const [showingDiff] = useContext(showDiffContext);

    if (showingDiff === true) {
        return html`<${PendingChanges}/>`;
    }

    return null;
}

export default ShowDiff;