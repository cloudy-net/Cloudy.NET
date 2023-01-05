import { html, useContext } from '@preact-htm';
import arrayEquals from '../util/array-equals';
import EntityContext from './entity-context';
import FieldComponentContext from "./field-component-context";

const FormField = ({ name, path, label, partial }) => {
    const fieldComponents = useContext(FieldComponentContext);

    if(!fieldComponents){
        return;
    }

    const { state } = useContext(EntityContext);
    const simpleChange = state.simpleChanges.find(c => arrayEquals(c.path, path));

    return html`<div class="mb-3">
    <label for=${name} class="form-label">${label} ${simpleChange ? '*' : null}</label>
    <${fieldComponents[partial]} name=${name} path=${path} />
    </div>`
};

export default FormField;