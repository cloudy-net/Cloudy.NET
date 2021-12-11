import PopupMenu from '../components/popup-menu/popup-menu.js';



/* CONTEXT MENU */

class ContextMenu {
    constructor() {
        this.button = document.createElement('cloudy-ui-context-menu-button');
        this.button.tabIndex = 0;
        this.button.addEventListener("keyup", event => {
            if (event.keyCode != 13) {
                return;
            }
            event.preventDefault();
            this.button.click();
        });
        this.button.addEventListener('click', () => this.toggle());

        this.popup = new PopupMenu(this.button);
    }

    toggle() {
        this.popup.toggle();
    }

    remove() {
        this.popup.remove();
    }

    setHorizontal() {
        this.popup.element.classList.add('cloudy-ui-horizontal');
        this.button.classList.add('cloudy-ui-horizontal');
        return this;
    }

    setCompact() {
        this.button.classList.add('cloudy-ui-compact');
        return this;
    }

    addItem(configurator) {
        this.popup.addItem(configurator);
        return this;
    }

    addSubHeader(text) {
        this.popup.addSubHeader(text);
        return this;
    }

    addSeparator() {
        this.popup.addSeparator();
        return this;
    }

    appendTo(element) {
        this.popup.appendTo(element);
        return this;
    }
}

export default ContextMenu;