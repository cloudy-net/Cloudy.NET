import { html, useContext } from '../../preact-htm/standalone.module.js';
import changeTracker from '../../data/state-manager.js';
import EntityContext from '../entity-context.js';

const Control = ({ name, value }) => {
    const { contentType, keys } = useContext(EntityContext);
    const onchange = event => {
        changeTracker.addChange(keys, contentType, {  })
        console.log(entityContext, event.target.value);
    };
    return html`<div>
        <input type="text" class="form-control" id=${`field-${name}`} name=${name} value=${value} onInput=${onchange}/>
    </div>`;
}

export default Control;