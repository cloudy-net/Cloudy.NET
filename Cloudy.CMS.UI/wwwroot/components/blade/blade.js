import html from '../../util/html.js';

function Blade({ onClose, title, toolbar, children }) {
    return html`
        <cloudy-ui-blade>
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