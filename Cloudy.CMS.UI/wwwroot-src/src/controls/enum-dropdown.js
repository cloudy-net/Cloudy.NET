
export default ({ name, path, validators, dependencies }) => {
  const {
    useState,
    useContext,
    EntityContext,
    urlFetcher,
    simpleChangeHandler,
    componentContextProvider,
    useEffect,
    html,
  } = dependencies;

  const [selectItems, setSelectItems] = useState([]);
  const { entityReference, state } = useContext(EntityContext);

  useEffect(function () {
    (async () => {
      const responseData = await urlFetcher.fetch(
        `/Admin/api/controls/select/enum/?entityType=${entityReference.value.entityType}&propertyName=${name}`,
        {
          credentials: 'include'
        },
        'Could not get select options'
      );

      setSelectItems(responseData);
    })();
  }, []);
  
  return html`<div>
      <select id=${componentContextProvider.getIdentifier(path)}
          class="form-control"
          value=${simpleChangeHandler.getIntermediateValue(state.value, path)}
          onChange=${e => simpleChangeHandler.setValue(entityReference.value, path, e.target.value, validators)}>
            ${selectItems.map(o => html`
              <option value=${o.value}>${o.text}</option>
            `)}
      </select>
    </div>`;
}