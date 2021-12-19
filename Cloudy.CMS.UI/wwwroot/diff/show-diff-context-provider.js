import html from '../util/html.js';
import ShowDiffContext from './show-diff-context.js';
import { useState } from '../lib/preact.hooks.module.js';

function ShowDiffContextProvider(props) {
    const state = useState();
    return html`
        <${ShowDiffContext.Provider} value=${state}>
            ${props.children}
        <//>
    `;
}

export default ShowDiffContextProvider;
