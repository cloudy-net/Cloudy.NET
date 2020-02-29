class FieldControl {
    constructor(element) {
        this.element = element;
        this.callbacks = {
            set: [],
            change: [],
            focus: [],
            blur: [],
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

    triggerFocus(value) {
        this.callbacks.focus.forEach(callback => callback(value));
    }

    onFocus(callback) {
        this.callbacks.focus.push(callback);

        return this;
    }

    triggerBlur(value) {
        this.callbacks.blur.forEach(callback => callback(value));
    }

    onBlur(callback) {
        this.callbacks.blur.push(callback);

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default FieldControl;