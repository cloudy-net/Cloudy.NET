const EmbeddedBlockFields = ({ type, path, dependencies }) => {
  const {
    useContext,
    html,
    FormField,
    ApplicationStateContext,
  } = dependencies;

  const { fieldTypes } = useContext(ApplicationStateContext);

  return fieldTypes.value[type].map(field => html`<${FormField} ...${field} path=${`${path}.${field.name}`} dependencies=${dependencies} />`);
};

export default EmbeddedBlockFields;