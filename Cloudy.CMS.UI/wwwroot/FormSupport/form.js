class Form {
    constructor(app, element, target, fields) {
        this.app = app;
        this.element = element;
        this.target = target;
        this.fields = fields;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default Form;