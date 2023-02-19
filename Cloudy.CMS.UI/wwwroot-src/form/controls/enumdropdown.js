import { html, useContext, useEffect, useState } from '../../preact-htm/standalone.module.js';
import EntityContext from '../entity-context.js';
import urlFetcher from '../../util/url-fetcher.js';
import simpleChangeHandler from '../../data/change-handlers/simple-change-handler.js';

export default ({ name, path, validators }) => {
  const [selectItems, setSelectItems] = useState([]);
  const { entityReference, state } = useContext(EntityContext);

  useEffect(function () {
    (async () => {
      const responseData = await urlFetcher.fetch(
        `/Admin/api/controls/select/enum/?entityType=${entityReference.entityType}&propertyName=${name}`,
        {
          credentials: 'include'
        },
        'Could not get select options'
      );

      setSelectItems(responseData);
    })();
  }, []);
  
  return html`<div>
  <select id=${name}
          name=${name}
          class="form-select"
          value=${simpleChangeHandler.getIntermediateValue(state, path)}
          onChange=${e => simpleChangeHandler.setValue(entityReference, path, e.target.value, validators)}>
            ${selectItems.map(o => html`
              <option value=${o.value}>${o.text}</option>
            `)}
      </select>
    </div>`;
}