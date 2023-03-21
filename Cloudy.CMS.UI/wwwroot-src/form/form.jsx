import FieldComponentProvider from './field-component-provider.js';
import EntityContextProvider from './entity-context-provider.js'
import FormFields from './form-fields.js';
import FormFooter from './form-footer.js';
import Changes from './changes.js';
import html from '@src/html-init.js';
import Card from '@src/layout/card.jsx';
import NewHeader from './form-header-new.jsx';
import EditHeader from './form-header-edit.jsx';
import { useState, useEffect } from 'preact/hooks';

import ValidationManager from '../data/validation-manager.js';

function Form({ entityTypeName, mode }) {
  const [error, setError] = useState();
  const [loading, setLoading] = useState(true);
  const [fields, setFields] = useState();
  const [keyValues, setKeyValues] = useState(null);

  useEffect(function () {
    const keyValuesFromUrl = new URL(document.location).searchParams.getAll('keys');
    if (keyValuesFromUrl && keyValuesFromUrl.length) {
      setKeyValues(keyValuesFromUrl);
    } else {
      setKeyValues(null);
    }

    return () => {

    };
  })();

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
  }, []);

  return keyValues && html`
    <${FieldComponentProvider}>
      <${EntityContextProvider} ...${{ entityType: entityTypeName, keyValues }}>
        <${mode === 'new' ? NewHeader : EditHeader} ...${{ entityTypeName, keyValues }} />
        <${Card}>
          <${Changes} />
          <${FormFields} ...${{ fields, error, loading }} />
          <${FormFooter} validateAll=${(entityReference) => ValidationManager.validateAll(fields, entityReference)} />
        </>
      </>
    </>`;
};

export default Form;