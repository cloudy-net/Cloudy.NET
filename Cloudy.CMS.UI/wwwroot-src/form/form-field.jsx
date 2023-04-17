import html from '@src/util/html.js';
import { useContext } from 'preact/hooks';
import EntityContext from './contexts/entity-context';
import FieldComponentContext from "./contexts/field-component-context";
import ValidationManager from '../data/validation-manager.js';

const FormField = ({ name, path, label, description, renderChrome, partial, listPartial, settings, validators, dependencies }) => {
    const { componentContextProvider } = dependencies;
    const fieldComponents = useContext(FieldComponentContext);

    if (!fieldComponents) {
        return;
    }

    const control = listPartial ?
        html`<${fieldComponents[listPartial]} ...${{ name, label, path, settings, validators, dependencies }} />` :
        html`<${fieldComponents[partial]} ...${{ name, label, path, settings, validators, dependencies }} />`;

    if (!renderChrome) {
        return control;
    }

    const { state } = useContext(EntityContext);

    return <div class={`${path.indexOf(".") == -1 ? "root-" : ""}form-field ${Object.keys(validators).length ? 'needs-validation' : ''} `}>
        <div class={`${path.indexOf(".") == -1 ? "root-" : ""}form-label`}>
            <label for={componentContextProvider.getIdentifier(path)}>{label} {state.changes.find(change => change.path == path) ? '*' : null}</label>
            {description && <small class={`${path.indexOf(".") == -1 ? "root-" : ""}form-description`}>{description}</small>}
        </div>
        <div class="form-control-outer">{control}</div>
        {Object.keys(validators).filter(v => ValidationManager.isInvalidForPathAndValidator(state.validationResults, path, v)).map(v =>
            <div class={`${path.indexOf(".") == -1 ? "root-" : ""}form-validation-error`}>{validators[v].message}</div>
        )}
    </div>;
};

export default FormField;