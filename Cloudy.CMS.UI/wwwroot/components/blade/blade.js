import html from '../../util/html.js';

function Blade(props) {
    return html`
        <cloudy-ui-blade>
            <cloudy-ui-blade-title>
                <cloudy-ui-blade-title-text>${props.title}<//>
                <cloudy-ui-blade-toolbar>${props.toolbar}<//>
                <cloudy-ui-blade-close onclick=${props.onclose}><//>
            <//>
            <cloudy-ui-blade-content>
                ${props.children}
            <//>
        <//>
    `;
}

export default Blade;