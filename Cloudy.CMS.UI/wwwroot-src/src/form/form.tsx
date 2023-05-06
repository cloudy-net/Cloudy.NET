import FieldComponentProvider from './contexts/field-component-provider';
import EntityContextProvider from './contexts/entity-context-provider'
import FormFields from './form-fields';
import Changes from './changes';
import NewHeader from './form-header-new';
import EditHeader from './form-header-edit';

function Form({ entityTypeName, mode, keyValues }: { entityTypeName: string, mode: string, keyValues: string[] }) {
  return <div class="form">
    <FieldComponentProvider>
      <EntityContextProvider {...{ entityType: entityTypeName, keyValues }}>
        {mode === 'new' ?
          <NewHeader {...{ entityTypeName }} /> :
          <EditHeader {...{ entityTypeName, keyValues }} />}
        <Changes />
        <div className="form-body">
          <FormFields {...{ entityTypeName }} />
        </div>
      </EntityContextProvider>
    </FieldComponentProvider>
  </div>;
};

export default Form;