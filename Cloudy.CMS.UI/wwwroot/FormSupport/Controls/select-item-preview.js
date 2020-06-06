


/* SELECT ITEM PREVIEW */

class SelectItemPreview {
    element = null;
    content = null;
    textContainer = null;
    text = null;
    callbacks = {
        click: [],
    };

    constructor() {
        this.element = document.createElement('cloudy-ui-select-preview');
        this.content = document.createElement('cloudy-ui-select-preview-content');
        this.element.append(this.content);

        this.content.tabIndex = 0;

        this.content.addEventListener("keyup", event => {
            if (event.keyCode != 13) {
                return;
            }

            event.preventDefault();
            this.content.click();
        });

        this.content.addEventListener('click', () => this.triggerClick());
    }

    triggerClick() {
        this.callbacks.click.forEach(callback => callback());
    }

    onClick(callback) {
        this.callbacks.click.push(callback);

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
            this.subText = document.createElement('cloudy-ui-list-item-sub-text');
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
            this.image.classList.add('cloudy-ui-list-item-image');
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