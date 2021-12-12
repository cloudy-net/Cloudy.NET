﻿import html from '../../util/html.js';

function Button(props) {
    return html`
        <cloudy-ui-button onclick=${props.onclick}>
            ${props.text}
        <//>
    `;
}

export default Button;

class Button2 {
    onClickCallbacks = [];

    constructor(text) {
        this.element = document.createElement('cloudy-ui-button');
        this.element.tabIndex = 0;
        this.element.innerText = text;
        this.initText = text;
        this.id = text && text.trim().toLocaleLowerCase().replace(' ', '_');
        this.element.addEventListener("keyup", event => {
            if (event.keyCode != 13) {
                return;
            }

            event.preventDefault();
            this.element.click();
        });

        this.element.addEventListener('click', () => this.triggerClick());
    }

    triggerClick() {
        this.onClickCallbacks.forEach(callback => callback());
    }

    onClick(callback) {
        this.onClickCallbacks.push(callback);

        return this;
    }

    addClass(value, test = true) {
        if (test) {
            this.element.classList.add(value);
        } else {
            this.element.classList.remove(value);
        }

        return this;
    }

    setText(text) {
        this.element.innerText = text;
    }

    setDisabled(test = true) {
        if (test) {
            this.element.setAttribute('disabled', '');
            this.element.removeAttribute('tabindex');
        } else {
            this.element.removeAttribute('disabled');
            this.element.tabIndex = 0;
        }

        return this;
    }

    setPrimary(value = true) {
        if (value) {
            this.element.classList.add('primary');
        } else {
            this.element.classList.remove('primary');
        }

        return this;
    }

    setStyle(values) {
        for(const [key, value] of Object.entries(values)){
            this.element.style[key] = value;
        }

        return this;
    }

    setInherit(value = true) {
        if (value) {
            this.element.classList.add('inherit');
        } else {
            this.element.classList.remove('inherit');
        }

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}
