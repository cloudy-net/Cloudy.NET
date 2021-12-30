import html from '../util/html.js';
import ShowDiffContext from './show-diff-context.js';
import { useState } from '../lib/preact.hooks.module.js';

function ShowDiffContextProvider(props) {
    const [diffData, setDiffData] = useState();
    
    return html`
        <${ShowDiffContext.Provider} value=${[diffData, setDiffData]}>
            ${props.children}
        <//>
    `;
}

export default ShowDiffContextProvider;
