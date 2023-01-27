import FieldComponentProvider from './field-component-provider.js';
import EntityContextProvider from './entity-context-provider.js'
import FormFields from './form-fields.js';
import FormFooter from './form-footer.js';
import ChangedContentWarning from './changed-content-warning.js';
import { html, useState,useEffect } from '../preact-htm/standalone.module.js';

import ValidationManager from '../data/validation-manager.js';

function Form({ entityType, keyValues }) {
  const [error, setError] = useState();
  const [loading, setLoading] = useState(true);
  const [fields, setFields] = useState();
  
  useEffect(function () {
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

  return html`<${FieldComponentProvider}>
    <${EntityContextProvider} ...${{ entityType, keyValues }}>
      <${ChangedContentWarning} />
      <${FormFields} ...${{ fields, error, loading }} />
      <${FormFooter} validateAll=${(entityReference) => ValidationManager.validateAll(fields, entityReference)} />
    <//>
  <//>`
};

export default Form;