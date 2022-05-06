import html from '../util/html.js';
import ShowDiffContext from './show-diff-context.js';
import { useEffect, useState } from '../lib/preact.hooks.module.js';
import stateManager from '../edit-content/state-manager.js';

function ShowDiffContextProvider({ renderIf, children, contentReference }) {
    if (!renderIf) {
        return;
    }

    const [state, setState] = useState(stateManager.getState(contentReference));

    useEffect(() => {
        setState(stateManager.getState(contentReference));

        const callback = () => setState({ ...stateManager.getState(contentReference) });
        stateManager.onStateChange(contentReference, callback);

        return () => {
            stateManager.offStateChange(contentReference, callback);
        };
    }, [contentReference]);

    return html`
        <${ShowDiffContext.Provider} value=${state}>
            ${children}
        <//>
    `;
}

export default ShowDiffContextProvider;
