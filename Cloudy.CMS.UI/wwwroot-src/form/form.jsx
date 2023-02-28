import FieldComponentProvider from './field-component-provider.js';
import EntityContextProvider from './entity-context-provider.js'
import FormFields from './form-fields.js';
import FormFooter from './form-footer.js';
import Changes from './changes.js';
import html from '@src/html-init.js';
import EntityTypesContext from '@src/form/entity-types-context';
import Card from '@src/layout/card.jsx';
import { useState, useEffect, useContext } from 'preact/hooks';

import ValidationManager from '../data/validation-manager.js';

function Form({ entityTypeName, mode }) {
  const [error, setError] = useState();
  const [loading, setLoading] = useState(true);
  const [fields, setFields] = useState();
  const [loaded, setLoaded] = useState(false);
  const [keyValues, setKeyValues] = useState(null);
  const [entityType, setEntityType] = useState({});
  const { entityTypes, getEntityTypeByTypeName } = useContext(EntityTypesContext);

  useEffect(() => {
    if (entityTypes.length) {
      setEntityType(getEntityTypeByTypeName(entityTypeName));
    }
  }, [entityTypes]);

  useEffect(function () {

    let keyValuesFromUrl = new URL(document.location).searchParams.getAll('keys');
    if (keyValuesFromUrl && keyValuesFromUrl.length) {
      setKeyValues(keyValuesFromUrl);
    }
    setLoaded(true);

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

  const NewHeader = () => <div class="container">
    <h1 class="h2 mb-3">
        { entityType.name }&nbsp;
        <a class="btn btn-sm btn-beta" href="/Admin/">Back</a>
    </h1>
  </div>;

  const EditHeader = () => <div class="container">
    <h1 class="h2 mb-3">
        { entityType.name }&nbsp;
        <a class="btn btn-sm btn-beta" href={`/Admin/List/${entityTypeName}`}>Back</a>&nbsp;
        <a class="btn btn-sm btn-primary" href={`/Admin/New/${entityTypeName}`}>New</a>
    </h1>
  </div>;
  
  return loaded && html`${ mode === 'new' ? <NewHeader/> : <EditHeader /> }
    <${Card}>
      <${FieldComponentProvider}>
        <${EntityContextProvider} ...${{ entityType : entityTypeName, keyValues }}>
          <${Changes} />
          <${FormFields} ...${{ fields, error, loading }} />
          <${FormFooter} validateAll=${(entityReference) => ValidationManager.validateAll(fields, entityReference)} />
        </>
      </>
    </>
  `
};

export default Form;