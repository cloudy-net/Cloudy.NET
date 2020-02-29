


/* LINK BUTTON */

class LinkButton {
    constructor(text, url, target) {
        this.element = document.createElement('a');
        this.element.classList.add('cloudy-ui-button');
        this.element.href = url;
        if (target) {
            this.element.target = target;
        }
        this.element.innerText = text;
    }

    addClass(value, test) {
        if (test) {
            this.element.classList.add(value);
        } else {
            this.element.classList.remove(value);
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

export default LinkButton;