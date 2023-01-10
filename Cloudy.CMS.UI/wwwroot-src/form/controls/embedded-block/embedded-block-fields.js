import { html, useEffect, useState } from '../../../preact-htm/standalone.module.js';
import FormField from '../../form-field.js';

const EmbeddedBlockFields = ({ type }) => {
    const [loading, setLoading] = useState(true);
    const [fields, setFields] = useState();
    const [error, setError] = useState();
    const [retryError, setRetryError] = useState(0);
  
    useEffect(function () {
      (async () => {
        setError(null);
  
        const response = await fetch(
          `/Admin/api/form/fields?type=${type}`,
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
  
    let content;
  
    if (error) {
      content = html`
        <div class="alert alert-primary">
          <p>
            There was an error (<code>${error.response.status}${error.response.statusText ? " " + error.response.statusText : ""}</code>)
            loading the block field types${error.body ? ":" : "."}
          </p>
          ${error.body ? html`<small><pre>{error.body}</pre></small>` : ""}
          <p class="mb-0"><button class="btn btn-primary" onClick=${() => { setError(null); setTimeout(() => setRetryError(retryError + 1), 500); }}>Reload</button></p>
        </div>
      `;
    }
  
    if (loading) {
      return html`Loading ...`;
    }

    return html`${fields.map(field => html`<FormField {...field} path={[field.name]} />`)}`;
};

export default EmbeddedBlockFields;