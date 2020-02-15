class DocumentActivityEvent {
    callbacks = [];

    constructor() {
        document.addEventListener('click', event => this.callbacks.forEach(callback => callback(event)));
        document.addEventListener('keyup', event => this.callbacks.forEach(callback => callback(event)));
    }

    addCallback(callback) {
        this.callbacks.push(callback);
    }
}

export default new DocumentActivityEvent();