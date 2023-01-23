import FieldComponentProvider from './field-component-provider.js';
import EntityContextProvider from './entity-context-provider'
import FormFields from './form-fields.jsx';
import FormFooter from './form-footer.jsx';
import ChangedContentWarning from './changed-content-warning.jsx';

function Form({ entityType, keyValues }) {
  return <FieldComponentProvider>
    <EntityContextProvider {...{ entityType, keyValues }}>
      <ChangedContentWarning />
      <FormFields type={entityType} />
      <FormFooter />
    </EntityContextProvider>
  </FieldComponentProvider>
};

export default Form;