import html from '../../util/html.js';

function Blade({ cssClass, onClose, title, toolbar, children }) {
    return html`
        <cloudy-ui-blade class=${cssClass}>
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