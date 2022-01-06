import html from '../util/html.js';
import ShowDiffContext from './show-diff-context.js';
import { useState } from '../lib/preact.hooks.module.js';

function ShowDiffContextProvider(props) {
    const [diffData, setDiffData] = useState();
    const [showDiffBlade, setShowDiffBlade] = useState(false);

    return html`
        <${ShowDiffContext.Provider} value=${[diffData, showDiffBlade, setDiffData, setShowDiffBlade]}>
            ${props.children}
        <//>
    `;
}

export default ShowDiffContextProvider;
