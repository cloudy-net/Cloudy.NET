import html from '../util/html.js';
import ListContentContext from './list-content-context.js';
import { useEffect, useState } from '../lib/preact.hooks.module.js';
import stateManager from '../edit-content/state-manager.js';

function ListContentContextProvider({ renderIf, children }) {
    if (!renderIf) {
        return;
    }

    const [states, setStates] = useState(stateManager.states);

    useEffect(() => {
        const callback = () => {
            setStates([...stateManager.states]);
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
