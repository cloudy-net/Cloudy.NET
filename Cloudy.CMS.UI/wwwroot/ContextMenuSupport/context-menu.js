import List from '../ListSupport/list.js';
import DocumentActivityEvent from '../DocumentActivityEvent.js';



/* CONTEXT MENU */

class ContextMenu {
    generators = [];

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

        var remove = () => {
            this.button.classList.remove('poetry-ui-active');
            this.menu.remove();
            this.menu = null;
            this.list = null;
        };

        this.button.addEventListener('click', () => {
            if (this.button.classList.contains('poetry-ui-active')) {
                remove();
                return;
            }

            this.button.classList.add('poetry-ui-active');

            this.menu = document.createElement('poetry-ui-context-menu');
            this.menu.style.opacity = 'none';
            this.list = new List();
            this.menu.append(this.list.element);
            document.body.append(this.menu);

            this.generators.forEach(generator => generator(this));

            this.menu.style.display = 'block';
            var offset = this.element.getBoundingClientRect();
            var menuOffset = this.menu.getBoundingClientRect();

            this.menu.style.top = `${offset.top - ((menuOffset.height - offset.height) / 2)}px`;
            this.menu.style.left = `${offset.left}px`;
        });

        this.element.appendChild(this.button);

        DocumentActivityEvent.addCallback(event => {
            if (!this.button.classList.contains('poetry-ui-active')) {
                return;
            }

            var found = false;

            for (var position = event.target; position && position != document; position = position.parentNode) {
                if (position == this.element) {
                    found = true;
                }
            }

            if (!found) {
                remove();
            }
        });
    }

    addItem(configurator) {
        this.generators.push(() => this.list.addItem(item => {
            configurator(item);
            item.onClick(() => {
                this.button.classList.remove('poetry-ui-active');
                this.menu.style.display = 'none';
            });
        }));
        return this;
    }

    addSubHeader(text) {
        this.generators.push(() => this.list.addSubHeader(text));
        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);
    }
}

export default ContextMenu;