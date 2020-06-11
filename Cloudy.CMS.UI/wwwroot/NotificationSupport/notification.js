


/* NOTIFICATION */

class Notification {
    onClickCallbacks = [];
    onCloseCallbacks = [];

    constructor() {
        this.element = document.createElement('cloudy-ui-notification');
        this.element.setAttribute('tabindex', 0);
        this.element.addEventListener('click', event => {
            if (event.target.matches('cloudy-ui-button')) {
                return;
            }
            if (window.getSelection().type == 'Range') {
                return;
            }

            this.triggerClick();
            this.close();
        });
        this.element.addEventListener('keyup', event => {
            if (event.keyCode != 13) {
                return;
            }

            event.preventDefault();
            this.triggerClick();
            this.close();
        });

        this.timeout = 10 * 1000;
        var startTimeout = () => this.timeoutReference = setTimeout(() => this.close(), this.timeout);
        var stopTimeout = () => clearTimeout(this.timeoutReference);
        startTimeout();

        this.element.addEventListener('mouseover', stopTimeout);
        this.element.addEventListener('mouseout', startTimeout);
    }

    close() {
        this.element.parentElement.removeChild(this.element);
        this.triggerClose();

        return this;
    }

    onClick(callback) {
        this.onClickCallbacks.push(callback);

        return this;
    }

    triggerClick() {
        this.onClickCallbacks.forEach(callback => callback());
    }

    setText(...items) {
        this.element.style.display = '';
        [...this.element.children].forEach(c => this.element.removeChild(c));
        items.forEach(item => {
            if (typeof item == 'string' && item.indexOf('`') != -1 && item.indexOf('`', item.indexOf('`') + 1) != -1) {
                var element = document.createElement('div');
                element.innerHTML = item.replace(/`([^`]+)`/, '<code>$1</code>');
                this.element.append(element);

                return;
            }
            this.element.append(item.element || item);
        });

        return this;
    }

    setTimeout(value) {
        this.timeout = value;

        return this;
    }

    onClose(callback) {
        this.onCloseCallbacks.push(callback);

        return this;
    }

    triggerClose(...parameters) {
        this.onCloseCallbacks.forEach(callback => callback(...parameters));
    }
}

export default Notification;