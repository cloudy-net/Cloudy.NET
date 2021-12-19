import html from '../util/html.js';
import PendingChangesContext from './pending-changes-context.js';
import { useState } from '../lib/preact.hooks.module.js';

function PendingChangesContextProvider(props) {
    const state = useState(true);
    return html`
        <${PendingChangesContext.Provider} value=${state}>
            ${props.children}
        <//>
    `;
}

export default PendingChangesContextProvider;
