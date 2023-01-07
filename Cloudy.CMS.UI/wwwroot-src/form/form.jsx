import FieldComponentProvider from './field-component-provider.jsx';
import EntityContextProvider from './entity-context-provider'
import FormFields from './form-fields.jsx';
import FormFooter from './form-footer.jsx';

function Form({ contentType, keyValues }) {
  return <FieldComponentProvider>
    <EntityContextProvider {...{ contentType, keyValues }}>
      <FormFields {...{ contentType }} />
      <FormFooter />
    </EntityContextProvider>
  </FieldComponentProvider>
};

export default Form;