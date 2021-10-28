class Form {
    onChangeCallbacks = [];

    constructor(element, fieldModels, fields) {
        this.element = element;
        this.fieldModels = fieldModels;
        this.fields = fields;
    }

    triggerChange(type, path, value) {
        this.onChangeCallbacks.forEach(callback => callback(type, path, value));
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