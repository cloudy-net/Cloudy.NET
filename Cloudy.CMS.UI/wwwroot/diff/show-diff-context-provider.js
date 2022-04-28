import html from '../util/html.js';
import ShowDiffContext from './show-diff-context.js';
import { useEffect, useState } from '../lib/preact.hooks.module.js';
import stateManager from '../edit-content/state-manager.js';

function ShowDiffContextProvider({ renderIf, children, contentReference }) {
    if (!renderIf) {
        return;
    }

    const state = stateManager.getState(contentReference);
    const [trigger, setTrigger] = useState(0);

    useEffect(() => {
        const callback = () => setTrigger(trigger + 1);
        stateManager.onStateChange(state.contentReference, callback);

        return () => {
            stateManager.offStateChange(state.contentReference, callback);
        };
    }, [contentReference]);

    return html`
        <${ShowDiffContext.Provider} value=${[state, trigger]}>
            ${children}
        <//>
    `;
}

export default ShowDiffContextProvider;
