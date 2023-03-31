import EmbeddedBlockFields from "../embedded-block/embedded-block-fields";

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

  const items = dependencies.embeddedBlockListHandler.getIntermediateValue(state, path);

  return html`
    <fieldset style="padding: 20px">
    ${items.map(item => html`<${EmbeddedBlockFields} ...${{ type: item.type, path: `${path}.${item.key}`, dependencies }}/>`)}
    <//>
    <${Dropdown} text="Add">
      ${types.map(type => dependencies.html`<a class="dropdown-item" onClick=${event => { dependencies.embeddedBlockListHandler.add(entityReference, path, type); dependencies.closeDropdown(event.target); }}>${type}</a>`)}
    <//>
  `;
};