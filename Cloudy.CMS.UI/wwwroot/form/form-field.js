import stateManager from '../data/state-manager.js';
import { html, useContext } from '../preact-htm/standalone.module.js';
import EntityContext from './entity-context.js';
import FieldComponentContext from "./field-component-context.js";

const FormField = ({ name, path, label, description, renderChrome, partial, settings }) => {
    const fieldComponents = useContext(FieldComponentContext);

    if(!fieldComponents){
        return;
    }
    
    if(!renderChrome){
        return html`<${fieldComponents[partial]} ...${{ name, label, path, settings }} />`;
    }

    const { state } = useContext(EntityContext);
    
    return html`<div class="mb-3">
    <label for=${name} class="form-label">${label} ${stateManager.hasChanges(state, path) ? '*' : null}</label>
    <${fieldComponents[partial]} ...${{ name, label, path, settings }} />
    ${ !!description ? html`<small class="form-text text-muted">${description}</small>` : '' }
    </div>`
};

export default FormField;