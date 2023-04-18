const EmbeddedBlockFields = ({ type, path, dependencies }) => {
  const {
    useState,
    useEffect,
    html,
    FormField,
    Dropdown,
    DropdownItem,
  } = dependencies;

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

  const kebab = html`<svg class="embedded-block-type-kebab" width="18" height="4" viewBox="0 0 18 4" fill="none" xmlns="http://www.w3.org/2000/svg"><rect x="14" width="4" height="4" rx="2" fill="#ABB0BB"/><rect x="7" width="4" height="4" rx="2" fill="#ABB0BB"/><rect width="4" height="4" rx="2" fill="#ABB0BB"/></svg>`;

  return html`<fieldset class="embedded-block">
      <legend class="embedded-block-type">
        ${type}
        <${Dropdown} contents=${kebab} className="embedded-block-type-button">
          <${DropdownItem} text="Remove" />
        <//>
      <//>
      ${fields.map(field => html`<${FormField} ...${field} path=${`${path}.${field.name}`} dependencies=${dependencies} />`)}
    <//>`;
};

export default EmbeddedBlockFields;