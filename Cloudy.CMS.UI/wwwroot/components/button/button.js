import html from '../../util/html.js';

function Button(props) {
    return html`
        <cloudy-ui-button class=${props.class} onclick=${props.onclick}>
            ${props.text}
        <//>
    `;
}

export default Button;