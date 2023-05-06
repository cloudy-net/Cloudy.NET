import { h } from 'preact';
import { useContext } from 'preact/hooks';
import EntityContext from './contexts/entity-context';
import FieldComponentContext from "./contexts/field-component-context";
import ValidationManager from '../data/validation-manager';
import FieldType from '../data/fieldtype.js';

const FormField = ({ fieldType, path, dependencies }: { fieldType: FieldType, path: string, dependencies: any }) => {
  const { componentContextProvider } = dependencies;
  const fieldComponents = useContext(FieldComponentContext);

  if (!fieldComponents) {
    return <></>;
  }

  const control = h(fieldType.listPartial ? fieldComponents[fieldType.listPartial] : fieldComponents[fieldType.partial], {
      name: fieldType.name,
      label: fieldType.label,
      settings: fieldType.settings,
      validators: fieldType.validators,
      path: path,
      dependencies: dependencies
  });

  if (!fieldType.renderChrome) {
    return control;
  }

  const { state } = useContext(EntityContext);

  return <div class={`${path.indexOf(".") == -1 ? "root-" : ""}form-field ${Object.keys(fieldType.validators).length ? 'needs-validation' : ''} `}>
    <div class={`${path.indexOf(".") == -1 ? "root-" : ""}form-label`}>
      <label for={componentContextProvider.getIdentifier(path)}>{fieldType.label} {state.value!.changes.find(change => change.path == path) ? '*' : null}</label>
      {fieldType.description && <small class={`${path.indexOf(".") == -1 ? "root-" : ""}form-description`}>{fieldType.description}</small>}
      {Object.keys(fieldType.validators).filter(v => ValidationManager.isInvalidForPathAndValidator(state.value!.validationResults, path, v)).map(v =>
        <div class={`${path.indexOf(".") == -1 ? "root-" : ""}form-validation-error`}>{fieldType.validators[v].message}</div>
      )}
    </div>
    <div class="form-control-outer">{control}</div>
  </div>;
};

export default FormField;