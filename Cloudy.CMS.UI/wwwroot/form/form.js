import FieldComponentProvider from './field-component-provider.js';
import EntityContextProvider from './entity-context-provider.js'
import FormFields from './form-fields.js';
import FormFooter from './form-footer.js';
import ChangedContentWarning from './changed-content-warning.js';
import { html } from '../preact-htm/standalone.module.js';

function Form({ entityType, keyValues }) {
  return html`<${FieldComponentProvider}>
    <${EntityContextProvider} ...${{ entityType, keyValues }}>
      <${ChangedContentWarning} />
      <${FormFields} type=${entityType} />
      <${FormFooter} />
    <//>
  <//>`
};

export default Form;