import { useEffect, useState } from '../lib/preact.hooks.module.js';
import html from '../util/html.js';
import StateContext from './state-context.js';
import stateManager from './state-manager.js';

function StateContextProvider({ renderIf, children, contentReference }) {
    if (!renderIf) {
        return;
    }

    const [state, setState] = useState(stateManager.getState(contentReference));

    useEffect(() => {
        setState(stateManager.getState(contentReference));
        
        const callback = () => {
            setState({ ...stateManager.getState(contentReference) });
        };
        stateManager.onStateChange(contentReference, callback);

        return () => {
            stateManager.offStateChange(contentReference, callback);
        };
    }, [contentReference]);

    return html`
        <${StateContext.Provider} value=${state}>
            ${children}
        <//>
    `;
}

export default StateContextProvider;
