import RemoveElementListener from '../remove-element-listener.js';
import List from '../ListSupport/list.js';



/* CONTEXT MENU */

class ContextMenu {
    constructor() {
        this.element = document.createElement('div');

        this.element.classList.add('poetry-ui-context-menu-outer');

        this.button = document.createElement('poetry-ui-context-menu-button');
        this.button.tabIndex = 0;
        this.button.addEventListener("keyup", event => {
            if (event.keyCode != 13) {
                return;
            }
            event.preventDefault();
            this.button.click();
        });

        this.button.addEventListener('click', () => {
            if (!this.button.classList.contains('poetry-ui-active')) {
                this.button.classList.add('poetry-ui-active');
                this.menu.style.display = '';
                this.menu.classList.remove('poetry-ui-context-menu-right');
                this.menu.classList.remove('poetry-ui-context-menu-bottom');

                if (!this.element.offsetParent || !this.menu.offsetParent) { // hidden or detached
                    return;
                }

                var menuOffset = this.menu.getBoundingClientRect();
                var windowRight = window.innerWidth + window.pageXOffset;
                var windowBottom = window.innerHeight + window.pageYOffset;

                if (menuOffset.right + window.pageXOffset > windowRight) {
                    this.menu.classList.add('poetry-ui-context-menu-right');
                } else {
                    this.menu.classList.remove('poetry-ui-context-menu-right');
                }

                if (menuOffset.bottom > windowBottom) {
                    this.menu.classList.add('poetry-ui-context-menu-bottom');
                }
            } else {
                this.button.classList.remove('poetry-ui-active');
                this.menu.style.display = 'none';
            }
        });

        this.element.appendChild(this.button);

        this.menu = document.createElement('poetry-ui-context-menu');
        this.menu.style.display = 'none';

        this.list = new List();
        this.menu.append(this.list.element);

        this.element.appendChild(this.menu);
    }

    addItem(configurator) {
        this.list.addItem(item => {
            configurator(item);
            item.onClick(() => {
                this.button.classList.remove('poetry-ui-active');
                this.menu.style.display = 'none';
            });
        });
        return this;
    }

    addSubHeader(text) {
        this.list.addSubHeader(text);
        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);
    }
}

export default ContextMenu;