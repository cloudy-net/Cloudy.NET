import { useEffect, useState } from '../../lib/preact.hooks.module.js';
import html from '../../util/html.js';
import FieldModelContext from './field-model-context.js';
import fieldModelProvider from './field-model-provider.js';

function FieldModelContextProvider({ children, contentReference }) {
    const [state, setState] = useState(null);

    useEffect(() => {
        fieldModelProvider.getFieldModels().then(result => setState(result));
    }, []);

    return html`
        <${FieldModelContext.Provider} value=${state}>
            ${state && children}
        <//>
    `;
}

export default FieldModelContextProvider;
