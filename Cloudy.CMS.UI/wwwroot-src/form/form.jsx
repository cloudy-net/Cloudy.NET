import FieldComponentProvider from './field-component-provider.jsx';
import EntityContextProvider from './entity-context-provider'
import FormFields from './form-fields.jsx';
import FormFooter from './form-footer.jsx';

function Form({ entityType, keyValues }) {
  return <FieldComponentProvider>
    <EntityContextProvider {...{ entityType, keyValues }}>
      <FormFields type={entityType} />
      <FormFooter />
    </EntityContextProvider>
  </FieldComponentProvider>
};

export default Form;