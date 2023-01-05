import FieldComponentProvider from './field-component-provider.jsx';
import EntityContextProvider from './entity-context-provider'
import FormFields from './form-fields.jsx';

function Form({ contentType, keyValues }) {
  return <FieldComponentProvider>
    <EntityContextProvider {...{ contentType, keyValues }}>
      <FormFields {...{ contentType }} />
      <button class="btn btn-primary" type='button'>Save</button>
    </EntityContextProvider>
  </FieldComponentProvider>
};

export default Form;