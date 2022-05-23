import { useEffect, useState } from '../../lib/preact.hooks.module.js';
import html from '../../util/html.js';
import FieldModelContext from './field-model-context.js';

function FieldModelContextProvider({ children, contentReference }) {
    const [state, setState] = useState(null);

    useEffect(() => {
        field
        setState(stateManager.getState(contentReference));
        
        
    }, []);

    return html`
        <${FieldModelContext.Provider} value=${state}>
            ${state && children}
        <//>
    `;
}

export default FieldModelContextProvider;
