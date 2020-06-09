import BladeCloseButton from './close-button.js';



/* BLADE */

class Blade {
    constructor() {
        this.element = document.createElement('cloudy-ui-blade');
        this.element.addEventListener('cloudy-ui-close-blade', event => {
            if (event.detail && event.detail.blade) {
                return;
            }

            event.stopPropagation();

            this.element.dispatchEvent(new CustomEvent('cloudy-ui-close-blade', { bubbles: true, detail: { blade: this, parameters: event.detail.parameters } }));
        });

        this._title = document.createElement('cloudy-ui-blade-title');
        this.element.append(this._title);
        this._titleText = document.createElement('cloudy-ui-blade-title-text');
        this._title.append(this._titleText);
        this._toolbar = document.createElement('cloudy-ui-blade-toolbar');
        this._title.append(this._toolbar);
        this._title.append(new BladeCloseButton().element);

        this._header = document.createElement('cloudy-ui-blade-header');
        this._header.style.display = 'none';
        this.element.append(this._header);

        this._content = document.createElement('cloudy-ui-blade-content');
        this.element.append(this._content);

        this._footer = document.createElement('cloudy-ui-blade-footer');
        this._footer.style.display = 'none';
        this.element.append(this._footer);
    }

    setFullWidth() {
        this.element.classList.add('cloudy-ui-fullwidth');
        return this;
    }

    setTitle(text) {
        this._titleText.innerText = text;

        return this;
    }

    setToolbar(...items) {
        this._toolbar.style.display = '';
        [...this._toolbar.children].forEach(c => this._toolbar.removeChild(c));
        items.forEach(item => this._toolbar.append(item.element || item));
    }

    setHeader(...items) {
        this._header.style.display = '';
        [...this._header.children].forEach(c => this._header.removeChild(c));
        items.forEach(item => this._header.append(item.element || item));
    }

    setContent(...items) {
        [...this._content.children].forEach(c => this._content.removeChild(c));
        items.forEach(item => this._content.append(item.element || item));
    }

    setFooter(...items) {
        this._footer.style.display = '';
        [...this._footer.children].forEach(c => this._footer.removeChild(c));
        items.forEach(item => this._footer.append(item.element || item));
    }

    open() {
        return Promise.resolve();
    }

    close(...parameters) {
        return Promise.resolve().then(() => this.triggerOnClose(...parameters));
    }

    onClose(callback) {
        this.onCloseCallback = callback;

        return this;
    }

    triggerOnClose(...parameters) {
        if (!this.onCloseCallback) {
            return;
        }

        this.onCloseCallback(...parameters);
    }
}

export default Blade;