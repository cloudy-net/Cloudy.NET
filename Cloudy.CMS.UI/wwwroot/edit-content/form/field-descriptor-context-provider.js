import { useEffect, useState } from '../../lib/preact.hooks.module.js';
import html from '../../util/html.js';
import urlFetcher from '../../util/url-fetcher.js';
import FieldDescriptorContext from './field-descriptor-context.js';

function FieldDescriptorContextProvider({ children }) {
    const [state, setState] = useState(null);

    useEffect(() => {
        urlFetcher.fetch(`Field/GetAll`, { credentials: 'include' }, `Could not get field descriptors`)
        .then(result => setState(result));
    }, []);

    return html`
        <${FieldDescriptorContext.Provider} value=${state}>
            ${state && children}
        <//>
    `;
}

export default FieldDescriptorContextProvider;
