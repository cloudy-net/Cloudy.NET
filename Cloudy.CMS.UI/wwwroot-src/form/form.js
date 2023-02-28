import FieldComponentProvider from './field-component-provider.js';
import EntityContextProvider from './entity-context-provider.js'
import FormFields from './form-fields.js';
import FormFooter from './form-footer.js';
import Changes from './changes.js';
import html from '@src/html-init.js';
import { useState, useEffect } from 'preact/hooks';

import ValidationManager from '../data/validation-manager.js';

function Form({ entityType, mode }) {
  const [error, setError] = useState();
  const [loading, setLoading] = useState(true);
  const [fields, setFields] = useState();
  const [loaded, setLoaded] = useState(false);
  const [keyValues, setKeyValues] = useState(null);

  useEffect(function () {

    let keyValuesFromUrl = new URL(document.location).searchParams.getAll('keys');
    if (keyValuesFromUrl && keyValuesFromUrl.length) {
      setKeyValues(keyValuesFromUrl);
    }
    setLoaded(true);

    (async () => {
      setError(null);

      const response = await fetch(
        `/Admin/api/form/fields?type=${entityType}`,
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
  }, []);

  const NewHeader = () => <div class="container">
  <h1 class="h2 mb-3">
      New @Model.EntityTypeName.LowerCaseName
      <a class="btn btn-sm btn-beta" href="#">Back</a>
  </h1>
</div>;
  
  return loaded && html`<${FieldComponentProvider}>
    ${ mode === 'new' ? <NewHeader/> : <div>edit</div> }
    <${EntityContextProvider} ...${{ entityType, keyValues }}>
      <${Changes} />
      <${FormFields} ...${{ fields, error, loading }} />
      <${FormFooter} validateAll=${(entityReference) => ValidationManager.validateAll(fields, entityReference)} />
    <//>
  <//>`
};

export default Form;