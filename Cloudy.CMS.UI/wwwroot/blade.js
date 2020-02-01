import BladeCloseButton from './close-button.js';



/* BLADE */

class Blade {
    constructor() {
        this.element = document.createElement('poetry-ui-blade');
        this.element.addEventListener('poetry-ui-close-blade', event => {
            if (event.detail && event.detail.blade) {
                return;
            }

            event.stopPropagation();

            this.element.dispatchEvent(new CustomEvent('poetry-ui-close-blade', { bubbles: true, detail: { blade: this, parameters: event.detail.parameters } }));
        });

        this.title = document.createElement('poetry-ui-blade-title');
        this.element.append(this.title);
        this.titleText = document.createElement('poetry-ui-blade-title-text');
        this.title.append(this.titleText);
        this.toolbar = document.createElement('poetry-ui-blade-toolbar');
        this.title.append(this.toolbar);
        this.title.append(new BladeCloseButton().element);

        this.header = document.createElement('poetry-ui-blade-header');
        this.header.style.display = 'none';
        this.element.append(this.header);

        this.content = document.createElement('poetry-ui-blade-content');
        this.element.append(this.content);

        this.footer = document.createElement('poetry-ui-blade-footer');
        this.footer.style.display = 'none';
        this.element.append(this.footer);
    }

    setFullWidth() {
        this.element.classList.add('poetry-ui-fullwidth');
        return this;
    }

    setTitle(text) {
        this.titleText.innerText = text;
    }

    setToolbar(...items) {
        this.toolbar.style.display = '';
        [...this.toolbar.children].forEach(c => this.toolbar.removeChild(c));
        items.forEach(item => this.toolbar.append(item.element || item));
    }

    setHeader(...items) {
        this.header.style.display = '';
        [...this.header.children].forEach(c => this.header.removeChild(c));
        items.forEach(item => this.header.append(item.element || item));
    }

    setContent(...items) {
        [...this.content.children].forEach(c => this.content.removeChild(c));
        items.forEach(item => this.content.append(item.element || item));
    }

    setFooter(...items) {
        this.footer.style.display = '';
        [...this.footer.children].forEach(c => this.footer.removeChild(c));
        items.forEach(item => this.footer.append(item.element || item));
    }

    open() {
        return new Promise(done => done());
    }

    close(...parameters) {
        return new Promise(done => done());
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