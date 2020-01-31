import RemoveElementListener from './remove-element-listener.js';



/* MENU */

class Menu {
    constructor() {
        this.element = document.createElement('div');

        this.element.classList.add('poetry-ui-menu-outer');

        var button = document.createElement('poetry-ui-menu-button');

        button.tabIndex = 0;

        button.addEventListener('click', () => {
            if (!button.classList.contains('poetry-ui-active')) {
                button.classList.add('poetry-ui-active');
                this.menu.style.display = '';

                update();
            } else {
                button.classList.remove('poetry-ui-active');
                this.menu.style.display = 'none';
            }
        });
        button.addEventListener("keyup", event => {
            if (event.keyCode != 13) {
                return;
            }

            event.preventDefault();
            button.click();
        });

        this.documentClickCallback = event => {
            if (event.target == button || event.target == this.menu || event.target.offsetParent == this.menu) {
                return;
            }

            button.classList.remove('poetry-ui-active');
            this.menu.style.display = 'none';
        };

        this.element.appendChild(button);

        this.menu = document.createElement('poetry-ui-menu');
        this.menu.style.display = 'none';

        this.element.appendChild(this.menu);
    }

    addItem(callback) {
        var item = new MenuItem();

        callback(item);

        item.onClick(() => this.menu.style.display = 'none');

        item.appendTo(this.menu);

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        document.addEventListener('click', this.documentClickCallback);
        this.setTimer();

        new RemoveElementListener(this.element, () => {
            document.removeEventListener('click', this.documentClickCallback);
            this.clearTimer();
        });
    }
}

class MenuItem {
    constructor() {
        this.element = document.createElement('poetry-ui-menu-item');
        this.element.tabIndex = 0;
        this.element.addEventListener("keyup", event => {
            if (event.keyCode != 13) {
                return;
            }

            event.preventDefault();
            this.element.click();
        });
    }

    setText(text) {
        this.element.innerText = text;

        return this;
    }

    setDisabled(value) {
        if (value) {
            this.element.setAttribute('poetry-ui-disabled', true);
            this.element.removeAttribute('tabindex');
        } else {
            this.element.removeAttribute('poetry-ui-disabled');
            this.element.tabIndex = 0;
        }
    }

    onClick(callback) {
        if (this.element.hasAttribute('poetry-ui-disabled')) {
            return;
        }

        this.element.addEventListener('click', callback);

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default Menu;