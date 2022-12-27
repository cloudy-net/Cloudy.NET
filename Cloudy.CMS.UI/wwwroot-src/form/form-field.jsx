import { useContext } from '@preact-htm';
import FieldComponentContext from "./field-component-context";

const FormField = ({ name, label, partial }) => {
    const fieldComponents = useContext(FieldComponentContext);

    if(!fieldComponents){
        return;
    }

    return <div class="mb-3">
    <label for={name} class="form-label">{label}</label>
    {fieldComponents[partial]()}
    </div>
};

export default FormField;