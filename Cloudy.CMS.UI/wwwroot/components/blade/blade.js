import { useEffect } from '../../lib/preact.hooks.module.js';
import { createRef } from '../../lib/preact.module.js';
import html from '../../util/html.js';

function Blade({ scrollIntoView, cssClass, onClose, title, toolbar, children }) {
    const ref = createRef(null);

    if (scrollIntoView) {
        useEffect(() => {
            ref.current.scrollIntoView({ block: "end", inline: "nearest", behavior: 'smooth' });
        }, scrollIntoView === true ? [] : Array.isArray(scrollIntoView) ? [...scrollIntoView] : [scrollIntoView]);
    }

    return html`
        <cloudy-ui-blade class=${cssClass} ref=${ref}>
            <cloudy-ui-blade-title>
                <cloudy-ui-blade-title-text>${title}<//>
                <cloudy-ui-blade-toolbar>${toolbar}<//>
                <cloudy-ui-blade-close onclick=${() => onClose && onClose()}><//>
            <//>
            ${children}
        <//>
    `;
}

export default Blade;