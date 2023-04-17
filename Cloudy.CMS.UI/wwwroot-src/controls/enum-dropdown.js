
export default ({ name, path, validators, dependencies }) => {
  const [selectItems, setSelectItems] = dependencies.useState([]);
  const { entityReference, state } = dependencies.useContext(dependencies.EntityContext);

  dependencies.useEffect(function () {
    (async () => {
      const responseData = await dependencies.urlFetcher.fetch(
        `/Admin/api/controls/select/enum/?entityType=${entityReference.entityType}&propertyName=${name}`,
        {
          credentials: 'include'
        },
        'Could not get select options'
      );

      setSelectItems(responseData);
    })();
  }, []);
  
  return dependencies.html`<div>
  <select id=${dependencies.componentContextProvider.getIdentifier(path)}
          class="form-control"
          value=${dependencies.simpleChangeHandler.getIntermediateValue(state, path)}
          onChange=${e => dependencies.simpleChangeHandler.setValue(entityReference, path, e.target.value, validators)}>
            ${selectItems.map(o => dependencies.html`
              <option value=${o.value}>${o.text}</option>
            `)}
      </select>
    </div>`;
}