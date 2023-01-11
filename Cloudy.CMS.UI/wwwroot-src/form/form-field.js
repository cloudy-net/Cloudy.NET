import { html, useContext } from '../preact-htm/standalone.module.js';
import arrayEquals from '../util/array-equals.js';
import EntityContext from './entity-context.js';
import FieldComponentContext from "./field-component-context.js";

const FormField = ({ name, path, label, renderChrome, partial, settings }) => {
    const fieldComponents = useContext(FieldComponentContext);

    if(!fieldComponents){
        return;
    }
    
    if(!renderChrome){
        return html`<${fieldComponents[partial]} ...${{ name, label, path, settings }} />`;
    }

    const { state } = useContext(EntityContext);
    const simpleChange = state.simpleChanges.find(c => arrayEquals(c.path, path));

    return html`<div class="mb-3">
    <label for=${name} class="form-label">${label} ${simpleChange ? '*' : null}</label>
    <${fieldComponents[partial]} ...${{ name, label, path, settings }} />
    </div>`
};

export default FormField;