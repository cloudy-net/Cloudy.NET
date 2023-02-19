import FormField from '../../form-field.js';

const EmbeddedBlockFields = ({ type, path, dependencies }) => {
    const [loading, setLoading] = dependencies.useState(true);
    const [fields, setFields] = dependencies.useState();
    const [error, setError] = dependencies.useState();
    const [retryError, setRetryError] = dependencies.useState(0);
  
    dependencies.useEffect(function () {
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
      content = dependencies.html`
        <div class="alert alert-primary">
          <p>
            There was an error (<code>${error.response.status}${error.response.statusText ? " " + error.response.statusText : ""}</code>)
            loading the block field types${error.body ? ":" : "."}
          </p>
          ${error.body ? dependencies.html`<small><pre>{error.body}</pre></small>` : ""}
          <p class="mb-0"><button class="btn btn-primary" onClick=${() => { setError(null); setTimeout(() => setRetryError(retryError + 1), 500); }}>Reload</button></p>
        </div>
      `;
    }
  
    if (loading) {
      return dependencies.html`Loading ...`;
    }

    return dependencies.html`${fields.map(field => dependencies.html`<${FormField} ...${field} path=${`${path}.${field.name}`} />`)}`;
};

export default EmbeddedBlockFields;