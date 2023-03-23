import html from '@src/util/html.js';
import { useContext } from 'preact/hooks';
import EntityContext from './contexts/entity-context';
import FieldComponentContext from "./contexts/field-component-context";
import ValidationManager from '../data/validation-manager.js';

const FormField = ({ name, path, label, description, renderChrome, partial, settings, validators, dependencies }) => {
    const { componentContextProvider } = dependencies;
    const fieldComponents = useContext(FieldComponentContext);

    if (!fieldComponents) {
        return;
    }

    if (!renderChrome) {
        return html`<${fieldComponents[partial]} ...${{ name, label, path, settings, validators, dependencies }} />`;
    }

    const { state } = useContext(EntityContext);

    return <div class={`mb-3 ${Object.keys(validators).length ? 'needs-validation' : ''} `}>
        <label for={componentContextProvider.getIdentifier(path)} class="form-label">{label} {state.changes.find(change => change.path == path) ? '*' : null}</label>
        {html`<${fieldComponents[partial]} ...${{ name, label, path, settings, validators, dependencies }} />`}
        {description && <small class="form-text text-muted">{description}</small>}
        {Object.keys(validators).filter(v => ValidationManager.isInvalidForPathAndValidator(state.validationResults, path, v)).map(v =>
            <div class="invalid-feedback">{validators[v].message}</div>
        )}
    </div>;
};

export default FormField;