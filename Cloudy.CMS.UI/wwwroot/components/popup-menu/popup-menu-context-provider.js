import html from '../../util/html.js';
import PopupMenuContext from './popup-menu-context.js';
import { useState } from '../../lib/preact.hooks.module.js';

function PopupMenuContextProvider(props) {
    const state = useState(null);
    return html`
        <${PopupMenuContext.Provider} value=${state}>
            ${props.children}
        <//>
    `;
}

export default PopupMenuContextProvider;
