import { createRef } from '../../lib/preact.module.js';
import html from '../../util/html.js';

function Text(props) {
    const { fieldModel, initialValue, onchange } = props;

    const ref = onchange && createRef();
    const changeEvent = onchange && (event => onchange(ref.current, event.srcElement.value));

    return html`
        <input ref=${ref} type=text class=cloudy-ui-form-input name=${fieldModel.descriptor.id} onclick=${props.onclick} value=${initialValue} onInput=${changeEvent} readonly=${props.readonly}/>
    `;
}

export default Text;