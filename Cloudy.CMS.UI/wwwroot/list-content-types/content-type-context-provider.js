import { useEffect, useState } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import urlFetcher from '../util/url-fetcher.js';
import ContentTypeContext from './content-type-context.js';

function ContentTypeContextProvider({ children }) {
    const [state, setState] = useState(null);

    useEffect(() => {
        urlFetcher
        .fetch('ContentTypeProvider/GetAll', { credentials: 'include' }, 'Could not get content types')
        .then(response => {
            setState(response.reduce((result, contentType) => { result[contentType.id] = contentType; return result; }, {}));
        });
    }, []);

    return html`
        <${ContentTypeContext.Provider} value=${state}>
            ${state && children}
        <//>
    `;
}

export default ContentTypeContextProvider;
