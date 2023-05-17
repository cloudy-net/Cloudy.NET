
const Name = ({ isImageable, image, keys, settings, value, dependencies }) => {

    return dependencies.html`
        ${isImageable && image && dependencies.html`<img class="list-image-in-name" src=${image} alt="" />`}
        ${isImageable && !image && dependencies.html`<div class="list-image-in-name list-image-in-name--image-not-set"><span class="list-image-in-name--image-not-set__bg"></span></div>`}
        <a class="name-link" href=${`${settings.editLink}?${keys.map(k => `keys=${k}`).join('&')}`}>${value}</a>
    `;
};

export default Name;