import { useContext } from 'preact/hooks';
import dependencies from '../dependencies.js';
import EntityContext from './contexts/entity-context.js';
import FormField from './form-field';
import ApplicationStateContext from '../application-state-context.js';

const FormFields = ({ entityTypeName }) => {
  const { state } = useContext(EntityContext);
  const { fieldTypes } = useContext(ApplicationStateContext);

  if(state.conflicts.length) {
    return;
  }

  if (fieldTypes.value.$loading) {
    return;
  }

  return fieldTypes.value[entityTypeName].map(field => <FormField {...field} path={field.name} dependencies={dependencies} />)
};

export default FormFields;