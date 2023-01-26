import stateManager from '../data/state-manager.js';
import { html, useContext } from '../preact-htm/standalone.module.js';
import EntityContext from './entity-context.js';
import FieldComponentContext from "./field-component-context.js";
import ValidationManager from '../data/validation-manager.js';

const FormField = ({ name, path, label, description, renderChrome, partial, settings, validators }) => {
    const fieldComponents = useContext(FieldComponentContext);

    if(!fieldComponents){
        return;
    }
    
    if(!renderChrome){
        return html`<${fieldComponents[partial]} ...${{ name, label, path, settings }} />`;
    }

    const { entityReference, state } = useContext(EntityContext);
    
    return html`<div class=${`mb-3 ${ Object.keys(validators).length ? 'needs-validation was-validated' : ''} `}>
        <label for=${name} class="form-label">${label} ${stateManager.hasChanges(state, path) ? '*' : null}</label>
        <${fieldComponents[partial]} ...${{ name, label, path, settings }} />
        ${ !!description ? html`<small class="form-text text-muted">${description}</small>` : '' }
        ${ Object.keys(validators).filter(v => !ValidationManager.isValid(v, path, entityReference)).map(v => html`
            <div class="invalid-feedback">${ validators[v].message }</div>`
        )} 
    </div>`
};

export default FormField;