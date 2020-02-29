


/* NOTIFICATION */

class Notification {
    constructor() {
        this.element = document.createElement('cloudy-ui-notification');
        this.element.setAttribute('tabindex', 0);
        this.element.addEventListener('click', event => {
            if (event.target.matches('cloudy-ui-button')) {
                return;
            }

            this.close();
        });
        this.element.addEventListener('keyup', event => {
            if (event.keyCode != 13) {
                return;
            }

            event.preventDefault();
            this.element.click();
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

        this.triggerOnClose();

        return this;
    }

    onClick(callback) {
        this.element.addEventListener('click', callback);

        return this;
    }

    setText(...items) {
        this.element.style.display = '';
        [...this.element.children].forEach(c => this.element.removeChild(c));
        items.forEach(item => this.element.append(item.element || item));

        return this;
    }

    setTimeout(value) {
        this.timeout = value;

        return this;
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

export default Notification;