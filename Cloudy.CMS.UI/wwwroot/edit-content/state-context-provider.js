import html from '../util/html.js';
import StateContext from './state-context.js';
import { useEffect, useState } from '../lib/preact.hooks.module.js';
import stateManager from './state-manager.js';

function StateContextProvider({ renderIf, children, contentReference }) {
    if (!renderIf) {
        return;
    }

    return html`
        <${StateContext.Provider} value=${stateManager.getState(contentReference)}>
            ${children}
        <//>
    `;
}

export default StateContextProvider;
