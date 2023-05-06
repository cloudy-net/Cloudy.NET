import { useContext } from 'preact/hooks';
import dependencies from '../dependencies.js';
import EntityContext from './contexts/entity-context';
import FormField from './form-field';
import ApplicationStateContext from '../application-state-context.js';

const FormFields = ({ entityTypeName }: { entityTypeName: string }) => {
  const { state } = useContext(EntityContext);
  const { fieldTypes } = useContext(ApplicationStateContext);

  if(state.value!.newSource) {
    return <></>;
  }

  if (!fieldTypes.value) {
    return <></>;
  }

  return <>{fieldTypes.value[entityTypeName].map(field => <FormField fieldType={field} path={field.name} dependencies={dependencies} />)}</>
};

export default FormFields;