export default ({ name, path, settings, validators, dependencies }) => {
  const {
    useState,
    useEffect,
    useContext,
    EntityContext,
    Dropdown,
    SelectEntityMenu,
    html,
    simpleChangeHandler,
  } = dependencies;

  const [value, setValue] = useState();
  const [preview, setPreview] = useState(null);
  const { entityReference, state } = useContext(EntityContext);

  const onChange = (newValue) => {
    setValue(newValue);
    simpleChangeHandler.setValue(entityReference.value, path, newValue, validators)
  };

  useEffect(() => {
    const value = simpleChangeHandler.getIntermediateValue(state.value, path);
    setValue(value);

    (async () => {
      if (!value) {
        setPreview(null);
        return;
      }

      if (preview && preview.reference == value) {
        return;
      }

      var response = await fetch(
        `/Admin/api/controls/select/preview?entityType=${settings.referencedTypeName}&reference=${value}&simpleKey=${settings.simpleKey}`,
        {
          credentials: 'include'
        }
      );

      if (response.status == 404) {
        setPreview({ notFound: true });
        return;
      }

      var json = await response.json();
      setPreview(json);
    })();
  }, [entityReference, value]);

  return html`
      <${Dropdown} contents=${preview && !preview.notFound && preview.name || "Add"}>
        <${SelectEntityMenu} entityType=${settings.referencedTypeName} simpleKey=${settings.simpleKey} value=${value} onSelect=${item => { onChange(settings.simpleKey ? (item && item.reference) : JSON.stringify(item.reference)); setPreview(item); }} />
      <//>
      ${value && html`<button class="button ml5" type="button" onClick=${() => { onChange(null); setPreview(null); }}>Remove</button>`}
      ${preview && !preview.notFound && html`<a class="button text ml5" href=${`/Admin/Edit/${settings.referencedTypeName}?${settings.simpleKey ? `keys=${preview.reference}` : preview.reference.map(key => `keys=${key}`).join('&')}`} target="_blank">Edit</a>`}
    `;
}
