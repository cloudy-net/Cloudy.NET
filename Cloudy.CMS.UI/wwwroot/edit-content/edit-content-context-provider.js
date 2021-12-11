import html from '../html.js';
import EditContentContext from './edit-content-context.js';
import { useState } from '../lib/preact.hooks.module.js';

function EditContentContextProvider(props) {
    const state = useState(null);
    return html`
        <${EditContentContext.Provider} value=${state}>
            ${props.children}
        <//>
    `;
}

export default EditContentContextProvider;
