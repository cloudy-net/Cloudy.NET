import html from '../util/html.js';
import ListContentContext from './list-content-context.js';
import { useEffect, useState } from '../lib/preact.hooks.module.js';
import stateManager from '../edit-content/state-manager.js';

function ListContentContextProvider({ renderIf, children }) {
    if (!renderIf) {
        return;
    }

    const [states, setStates] = useState(stateManager.getAll());

    useEffect(() => {
        const callback = () => {
            setStates(stateManager.getAll());
        };
        stateManager.onAnyStateChange(callback);

        return () => {
            stateManager.offAnyStateChange(callback);
        };
    }, []);

    return html`
        <${ListContentContext.Provider} value=${states}>
            ${children}
        <//>
    `;
}

export default ListContentContextProvider;
