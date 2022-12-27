import { html } from '/_content/Cloudy.CMS.UI/preact-htm/standalone.module.js';

const Control = ({ name, value }) => {
    return html`<div>
        <input type="text" class="form-control" id=${`field-${name}`} name=${name} value=${value}/>
    </div>`;
}

export default Control;