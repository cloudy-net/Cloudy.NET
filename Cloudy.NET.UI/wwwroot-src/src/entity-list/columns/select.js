
const Select = ({ value, dependencies }) => {
    return value && dependencies.html`${value.image && dependencies.html`<img class="list-image-in-name" src=${value.image} alt="" />`}${value.name}`;
};

export default Select;