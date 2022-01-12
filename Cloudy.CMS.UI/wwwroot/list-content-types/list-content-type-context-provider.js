import html from '../util/html.js';
import ListContentTypeContext from './list-content-type-context.js';
import { useState } from '../lib/preact.hooks.module.js';

function ListContentTypeContextProvider(props) {
    const state = useState(null);
    return html`
        <${ListContentTypeContext.Provider} value=${state}>
            ${props.children}
        <//>
    `;
}

export default ListContentTypeContextProvider;
