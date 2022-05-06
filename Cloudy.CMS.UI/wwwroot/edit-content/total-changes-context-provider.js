import html from '../util/html.js';
import StateContext from './total-changes-context.js';
import { useEffect, useState } from '../lib/preact.hooks.module.js';
import stateManager from './state-manager.js';

function TotalChangesContextProvider({ children }) {
    const [totalChanges, setTotalChanges] = useState(stateManager.getAll().length);

    useEffect(() => {
        const callback = () => {
            setTotalChanges(stateManager.getAll().length);
        };
        stateManager.onAnyStateChange(callback);

        return () => {
            stateManager.offAnyStateChange(callback);
        };
    }, []);

    return html`
        <${StateContext.Provider} value=${totalChanges}>
            ${children}
        <//>
    `;
}

export default TotalChangesContextProvider;
