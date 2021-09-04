class Form {
    constructor(app, element, target, fields) {
        this.app = app;
        this.element = element;
        this.target = target;
        this.fields = fields;
        this.callbacks = {
            change: [],
        };
    }

    triggerChange(path, value) {
        this.callbacks.change.forEach((callback) => callback(path, value));
    }

    onChange(callback) {
        this.callbacks.change.push(callback);

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default Form;