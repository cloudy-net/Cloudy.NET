import html from '../util/html.js';
import EditContentContext from './edit-content-reference-context.js';
import { useEffect, useState } from '../lib/preact.hooks.module.js';

function EditContentContextProvider({ children }) {
    const [editingContent, setEditingContent] = useState();

    return html`
        <${EditContentContext.Provider} value=${[editingContent, setEditingContent]}>
            ${children}
        <//>
    `;
}

export default EditContentContextProvider;
