import FieldComponentProvider from './contexts/field-component-provider.js';
import EntityContextProvider from './contexts/entity-context-provider.js'
import FormFields from './form-fields.js';
import FormFooter from './form-footer.js';
import Changes from './changes';
import Card from '@src/layout/card.jsx';
import NewHeader from './form-header-new.jsx';
import EditHeader from './form-header-edit.jsx';
import { useState, useEffect } from 'preact/hooks';

import ValidationManager from '../data/validation-manager.js';

function Form({ entityTypeName, mode, keyValues }) {
  const [error, setError] = useState();
  const [retryError, setRetryError] = useState(0);
  const [loading, setLoading] = useState(true);
  const [fields, setFields] = useState();

  useEffect(function () {
    (async () => {
      setError(null);

      const response = await fetch(
        `/Admin/api/form/fields?type=${entityTypeName}`,
        {
          credentials: 'include'
        }
      );

      if (!response.ok) {
        setError({ response, body: await response.text() });
        return;
      }

      var json = await response.json();

      setLoading(false);
      setFields(json);
    })();
  }, [retryError]);

  if (error) {
    return <div class="alert alert-primary">
      <p>
        There was an error (<code>{error.response.status}{error.response.statusText ? " " + error.response.statusText : ""}</code>)
        loading the entity field types{error.body ? ":" : "."}
      </p>
      {error.body ? <small><pre>{error.body}</pre></small> : ""}
      <p class="mb-0"><button class="btn btn-primary" onClick={() => { setError(null); setTimeout(() => setRetryError(retryError + 1), 500); }}>Reload</button></p>
    </div>
  }

  return <FieldComponentProvider>
    <EntityContextProvider {...{ entityType: entityTypeName, keyValues }}>
      {mode === 'new' ? <NewHeader {...{ entityTypeName, keyValues }} /> : <EditHeader {...{ entityTypeName, keyValues }} />}
      <Card>
        <Changes />
        <FormFields {...{ fields, error, loading }} />
        <FormFooter validateAll={(entityReference) => ValidationManager.validateAll(fields, entityReference)} />
      </Card>
    </EntityContextProvider>
  </FieldComponentProvider>;
};

export default Form;