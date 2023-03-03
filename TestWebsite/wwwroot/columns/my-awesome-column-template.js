
// Use by adding [ListColumn("/columns/my-awesome-column-template.js")]

const Control = ({ value, dependencies }) => dependencies.html`<div style="min-height: 32px; border: 1px solid green; border-radius: 5px; padding: 5px;">
    ${ value }
</div>`;

export default Control;