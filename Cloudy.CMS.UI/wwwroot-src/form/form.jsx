import FieldComponentProvider from './contexts/field-component-provider';
import EntityContextProvider from './contexts/entity-context-provider'
import FormFields from './form-fields';
import Changes from './changes';
import NewHeader from './form-header-new.jsx';
import EditHeader from './form-header-edit.jsx';
import { useState, useEffect } from 'preact/hooks';

function Form({ entityTypeName, mode, keyValues }) {
  return <div class="form">
    <FieldComponentProvider>
      <EntityContextProvider {...{ entityType: entityTypeName, keyValues }}>
        {mode === 'new' ?
          <NewHeader {...{ entityTypeName }} /> :
          <EditHeader {...{ entityTypeName, keyValues }} />}
        <div className="form-body">
          <Changes />
          <FormFields {...{ entityTypeName }} />
        </div>
      </EntityContextProvider>
    </FieldComponentProvider>
  </div>;
};

export default Form;