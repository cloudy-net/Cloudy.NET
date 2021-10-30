class Form {
    constructor(element, fieldModels, fields, eventDispatcher) {
        this.element = element;
        this.fieldModels = fieldModels;
        this.fields = fields;
        this.eventDispatcher = eventDispatcher;
    }

    triggerChange() {
        this.eventDispatcher.triggerChange(...arguments);
    }
    onChange(callback) {
        this.eventDispatcher.onChange(callback);

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default Form;