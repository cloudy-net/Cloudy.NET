export default ({ controlId, controlName, nullable, value, options }) => {
  return <>
    <select class="form-select" id={controlId} name={controlName} value={value}>
      {nullable && <option value="">(empty)</option>}
      {(options ? Object.entries(options) : []).map(([name, label]) => <option value={name}>{label}</option>)}
    </select>
  </>;
};