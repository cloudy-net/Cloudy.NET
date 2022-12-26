import { useContext } from '@preact-htm';
import FieldComponentContext from "./field-component-context";

const FormField = ({ name, label }) => {
    const fieldComponents = useContext(FieldComponentContext);
    return <div class="mb-3">
    <label for={name} class="form-label">{label}</label>
    {fieldComponents}
    </div>
};

export default FormField;