import html from '../util/html.js';
import ListContentTypeContext from './list-content-type-context.js';
import { useState } from '../lib/preact.hooks.module.js';

function ListContentTypeContextProvider(props) {
    const [listContentType, setListContentType] = useState(null);
    const [singleton, setSingleton] = useState(false);

    return html`
        <${ListContentTypeContext.Provider} value=${[listContentType, setListContentType, singleton, setSingleton]}>
            ${props.children}
        <//>
    `;
}

export default ListContentTypeContextProvider;
