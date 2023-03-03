
const Image = ({ value, dependencies }) => {

    return value && dependencies.html`<img class="list-image" src=${value} alt="" />`;
};

export default Image;