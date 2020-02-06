class FieldControl {
    static customLabel = false;

    constructor(element) {
        this.element = element;
        this.callbacks = {
            set: [],
            change: [],
        };
    }

    triggerChange(value) {
        this.callbacks.change.forEach(callback => callback(value));
    }

    onChange(callback) {
        this.callbacks.change.push(callback);

        return this;
    }

    triggerSet(value) {
        this.callbacks.set.forEach(callback => callback(value));
    }

    onSet(callback) {
        this.callbacks.set.push(callback);

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default FieldControl;