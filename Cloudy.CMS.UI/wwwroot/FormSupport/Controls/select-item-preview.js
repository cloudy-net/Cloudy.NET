


/* SELECT ITEM PREVIEW */

class SelectItemPreview {
    element = null;
    content = null;
    textContainer = null;
    text = null;
    onClickCallbacks = [];

    constructor() {
        this.element = document.createElement('cloudy-ui-select-preview');
        this.content = document.createElement('cloudy-ui-select-preview-content');
        this.element.append(this.content);

        this.content.addEventListener('click', () => this.triggerClick());
    }

    triggerClick() {
        this.onClickCallbacks.forEach(callback => callback());
    }

    onClick(callback) {
        this.onClickCallbacks.push(callback);

        return this;
    }

    setText(value) {
        if (!this.textContainer) {
            this.textContainer = document.createElement('cloudy-ui-select-preview-text-container');
            this.content.append(this.textContainer);
        }
        if (!this.text) {
            this.text = document.createElement('cloudy-ui-select-preview-text');
            this.textContainer.append(this.text);
        }

        if (!value) {
            this.text.style.display = 'none';
            return;
        }

        this.text.style.display = '';
        this.text.innerHTML = value;

        return this;
    }

    setSubText(value) {
        if (!this.textContainer) {
            this.textContainer = document.createElement('cloudy-ui-select-preview-text-container');
            this.content.append(this.textContainer);
        }
        if (!this.subText) {
            this.subText = document.createElement('cloudy-ui-select-preview-sub-text');
            this.textContainer.append(this.subText);
        }

        if (!value) {
            this.subText.style.display = 'none';
            return;
        }

        this.subText.style.display = '';
        this.subText.innerHTML = value;

        return this;
    }

    setImage(value) {
        if (!this.image) {
            this.image = document.createElement('img');
            this.image.classList.add('cloudy-ui-select-preview-image');
            this.content.insertBefore(this.image, this.textContainer);
        }

        if (!value) {
            this.image.style.display = 'none';
            return;
        }

        this.image.style.display = '';
        this.image.src = value;

        return this;
    }

    setMenu(menu) {
        this.menu = menu;
        this.menu.appendTo(this.element);
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default SelectItemPreview;