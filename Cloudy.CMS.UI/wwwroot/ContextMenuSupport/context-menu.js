import List from '../ListSupport/list.js';
import DocumentActivityEvent from '../DocumentActivityEvent.js';



/* CONTEXT MENU */

class ContextMenu {
    generators = [];

    constructor() {
        this.element = document.createElement('div');

        this.element.classList.add('cloudy-ui-context-menu-outer');

        this.button = document.createElement('cloudy-ui-context-menu-button');
        this.button.tabIndex = 0;
        this.button.addEventListener("keyup", event => {
            if (event.keyCode != 13) {
                return;
            }
            event.preventDefault();
            this.button.click();
        });

        var remove = () => {
            this.button.classList.remove('cloudy-ui-active');
            this.menu.remove();
            this.menu = null;
            this.list = null;
        };

        this.button.addEventListener('click', () => {
            if (this.button.classList.contains('cloudy-ui-active')) {
                remove();
                return;
            }

            this.button.classList.add('cloudy-ui-active');

            this.menu = document.createElement('cloudy-ui-context-menu');
            this.menu.style.opacity = 'none';
            this.list = new List();
            this.menu.append(this.list.element);
            document.body.append(this.menu);

            this.generators.forEach(generator => generator(this));

            this.menu.style.display = 'block';
            var offset = this.element.getBoundingClientRect();
            var menuOffset = this.menu.getBoundingClientRect();

            this.menu.style.top = `${offset.top - ((menuOffset.height - offset.height) / 2)}px`;

            var left = offset.left + window.pageXOffset;

            if (left == 0) {
                left = -4;
            }

            if (left + menuOffset.width > window.innerWidth + window.pageXOffset) {
                left = window.innerWidth + window.pageXOffset - menuOffset.width + 4;
            }

            this.menu.style.left = `${left}px`;
        });

        this.element.appendChild(this.button);

        DocumentActivityEvent.addCallback(event => {
            if (!this.button.classList.contains('cloudy-ui-active')) {
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
                this.button.classList.remove('cloudy-ui-active');
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