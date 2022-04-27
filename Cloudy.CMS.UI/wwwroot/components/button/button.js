import html from '../../util/html.js';

function Button({ cssClass, onClick, disabled, text }) {
    return html`
        <button type="button" class="cloudy-ui-button ${cssClass}" onclick=${onClick} disabled=${disabled}>
            ${text}
        <//>
    `;
}

export default Button;