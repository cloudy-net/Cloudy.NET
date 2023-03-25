export default ({ name, path, provider, dependencies, settings: { types } }) => {
  const {
    html,
    useContext,
    useState,
    simpleChangeHandler,
    EntityContext,
    Dropdown,
    closeDropdown,
  } = dependencies;

  const { entityReference, state } = useContext(EntityContext);

  return html`
    <${Dropdown} text="Add">
      ${types.map(type => dependencies.html`<a class="dropdown-item" onClick=${event => { dependencies.blockTypeChangeHandler.setType(entityReference, path, type); dependencies.closeDropdown(event.target); }}>${type}</a>`)}
    <//>
  `;
};