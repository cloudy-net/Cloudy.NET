import { useEffect, useState } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import ContentTypeContext from './content-type-context.js';
import contentTypeProvider from './content-type-provider.js';

function ContentTypeContextProvider({ children }) {
    const [state, setState] = useState(null);

    useEffect(() => {
        contentTypeProvider.fetch().then(() => setState(contentTypeProvider.allById));
    }, []);

    return html`
        <${ContentTypeContext.Provider} value=${state}>
            ${state && children}
        <//>
    `;
}

export default ContentTypeContextProvider;
