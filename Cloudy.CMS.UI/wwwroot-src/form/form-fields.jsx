import { useContext } from 'preact/hooks';
import dependencies from '../dependencies.js';
import EntityContext from './contexts/entity-context.js';
import FormField from './form-field';

const FormFields = ({ fields, error, loading }) => {
  const { state } = useContext(EntityContext);

  if(state.conflicts.length) {
    return;
  }

  if (loading) {
    return;
  }

  return fields.map(field => <FormField {...field} path={field.name} dependencies={dependencies} />)
};

export default FormFields;