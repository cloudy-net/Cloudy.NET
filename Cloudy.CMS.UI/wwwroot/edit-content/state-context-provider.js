import html from '../util/html.js';
import StateContext from './state-context.js';
import { useEffect, useState } from '../lib/preact.hooks.module.js';
import stateManager from './state-manager.js';

function StateContextProvider({ children, contentReference }) {
    if (!contentReference) {
        return;
    }

    const [state, setState] = useState(stateManager.getState(contentReference));

    useEffect(() => {
        const callback = () => {
            setState({ ...stateManager.getState(contentReference) });
        };
        stateManager.onStateChange(contentReference, callback);

        return () => {
            stateManager.offStateChange(contentReference, callback);
        };
    }, []);

    return html`
        <${StateContext.Provider} value=${state}>
            ${children}
        <//>
    `;
}

export default StateContextProvider;
