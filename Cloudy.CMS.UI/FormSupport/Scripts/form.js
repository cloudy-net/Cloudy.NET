class Form {
    constructor(app, element, target, fields) {
        this.app = app;
        this.element = element;
        this.target = target;
        this.fields = fields;
    }

    getValues() {
        return this.getObject(this.fields, this.target);
    }

    copy() {
        navigator.clipboard.writeText(JSON.stringify(this.getValues(), null, '  '));
    }

    getObject(fields, target) {
        var result = {};

        fields.forEach(field => result[field.model.descriptor.Id] = this.getField(field, target));

        return result;
    }

    getField(field, target) {
        if (field.model.descriptor.IsSortable) {
            return this.getSortableField(field, target[field.model.descriptor.Id]);
        }

        if (field.model.descriptor.EmbeddedFormId) {
            return this.getObject(field.data.form.fields, target[field.model.descriptor.Id]);
        }

        return target[field.model.descriptor.Id];
    }

    getSortableField(field, target) {
        var result = [];

        if (field.model.descriptor.EmbeddedFormId) {
            target.forEach((_, i) => result[i] = this.getObject(field.data.sortable.items[0].data.form.fields, target[i]));

            return result;
        } else {
            target.forEach((_, i) => result[i] = target[i]);

            return result;
        }
    }

    paste() {
        navigator.clipboard.readText().then(value => {
            var value = JSON.parse(value);

            this.updateFields(this.fields, this.target, value);
        });
    }

    updateFields(fields, target, value) {
        fields.forEach(field => {
            if (field.model.descriptor.IsSortable) {
                this.updateSortable(field, target[field.model.descriptor.Id], value[field.model.descriptor.Id]);

                return;
            }

            if (field.model.descriptor.EmbeddedFormId) {
                this.updateFields(field.data.form.fields, target[field.model.descriptor.Id], value[field.model.descriptor.Id]);

                return;
            }

            target[field.model.descriptor.Id] = value[field.model.descriptor.Id];
            field.data.control.triggerSet(value[field.model.descriptor.Id]);
        });
    }

    updateSortable(field, target, value) {
        if (field.model.descriptor.EmbeddedFormId) {
            while (value.length > target.length) {
                target.push({});
                field.data.sortable.addItem(field.data.sortable.createItem());
            }

            value.forEach((v, i) => {
                if (!target[i]) {
                    target[i] = {};
                }

                this.updateFields(field.data.sortable.items[i].data.form.fields, target[i], value[i]);
            });

            return;
        }

        target.forEach((_, i) => {
            target[i] = value[i];
            field.data.sortable.items[i].data.control.triggerSet(value[i]);
        });
    }
}

export default Form;