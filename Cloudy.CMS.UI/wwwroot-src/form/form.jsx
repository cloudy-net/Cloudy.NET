import FieldComponentProvider from './contexts/field-component-provider';
import EntityContextProvider from './contexts/entity-context-provider'
import FormFields from './form-fields';
import Changes from './changes';
import NewHeader from './form-header-new.jsx';
import EditHeader from './form-header-edit.jsx';
import { useState, useEffect } from 'preact/hooks';

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

  return <div class="form">
    <FieldComponentProvider>
      <EntityContextProvider {...{ entityType: entityTypeName, keyValues }}>
        {mode === 'new' ?
          <NewHeader {...{ entityTypeName, keyValues }} /> :
          <EditHeader {...{ entityTypeName, keyValues }} />}
        <div className="form-body">
          <Changes />
          <FormFields {...{ fields, error, loading }} />
        </div>
      </EntityContextProvider>
    </FieldComponentProvider>
  </div>;
};

export default Form;