import { html, useContext } from '../../preact-htm/standalone.module.js';
import stateManager from '../../data/state-manager.js';
import EntityContext from '../entity-context.js';

const Control = ({ name, path, value }) => {
    const { reference } = useContext(EntityContext);
    const onchange = event => {
        stateManager.registerSimpleChange(reference, path, event.target.value)
    };
    return html`<div>
        <input type="text" class="form-control" id=${`field-${name}`} name=${name} value=${value} onInput=${onchange}/>
    </div>`;
}

export default Control;