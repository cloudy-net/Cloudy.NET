class FieldControl {
    constructor(element) {
        this.element = element;
        this.callbacks = {
            set: [],
            change: [],
            enlargeLabel: [],
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

    enlargeLabel = false;

    setEnlargeLabel(value = true) {
        this.enlargeLabel = value;
        this.callbacks.enlargeLabel.forEach(callback => callback(value));
    }

    onSetEnlargeLabel(callback) {
        this.callbacks.enlargeLabel.push(callback);

        return this;
    }
}

export default FieldControl;