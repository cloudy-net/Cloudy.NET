class Form {
    onChangeCallbacks = [];

    constructor(element, fieldModels, fields) {
        this.element = element;
        this.fieldModels = fieldModels;
        this.fields = fields;
    }

    triggerChange() {
        this.onChangeCallbacks.forEach(callback => callback(...arguments));
    }
    onChange(callback) {
        this.onChangeCallbacks.push(callback);

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default Form;