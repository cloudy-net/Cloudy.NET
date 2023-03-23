import html from '@src/html-init.js';
import { useContext } from 'preact/hooks';
import dependencies from '../dependencies.js';
import EntityContext from './contexts/entity-context.js';
import FormField from './form-field.js';

const FormFields = ({ fields, error, loading }) => {
  const { state } = useContext(EntityContext);

  if(state.conflicts.length) {
    return;
  }

  if (loading) {
    return;
  }

  return fields.map(field => html`<${FormField} ...${field} path=${field.name} dependencies=${dependencies} />`)
};

export default FormFields;