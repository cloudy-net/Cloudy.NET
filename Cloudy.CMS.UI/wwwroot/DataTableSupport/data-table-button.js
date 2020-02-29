


/* DATA TABLE BUTTON */

class DataTableButton {
    constructor(text, url = null, target = null) {
        if (url) {
            this.element = document.createElement('a');
            this.element.classList.add('cloudy-ui-data-table-button');
            this.element.setAttribute('href', url);

            if (target) {
                this.element.setAttribute('target', target);
            }
        } else {
            this.element = document.createElement('cloudy-ui-data-table-button');
            this.element.tabIndex = 0;
            this.element.addEventListener("keyup", event => {
                if (event.keyCode != 13) {
                    return;
                }

                event.preventDefault();
                this.element.click();
            });

            this.callbacks = {
                click: [],
            };

            this.element.addEventListener('click', () => this.triggerClick());
        }

        this.element.innerText = text;
    }

    triggerClick() {
        if (this.element.nodeName == 'A') {
            this.element.click();
        } else {
            this.callbacks.click.forEach(callback => callback());
        }
    }

    onClick(callback) {
        if (this.element.nodeName == 'A') {
            this.element.addEventListener('click', callback);
        } else {
            this.callbacks.click.push(callback);
        }

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

    setActive(test = true) {
        if (test) {
            this.element.classList.add('cloudy-ui-active');
        } else {
            this.element.classList.remove('cloudy-ui-active');
        }

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default DataTableButton;