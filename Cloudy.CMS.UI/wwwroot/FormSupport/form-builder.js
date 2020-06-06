import Form from './form.js';
import Field from './field.js';
import FieldModel from './field-model.js';
import FieldDescriptorProvider from './field-descriptor-provider.js';
import FieldControlProvider from './field-control-provider.js';
import Sortable from './sortable.js';
import SortableItem from './sortable-item.js';



/* FORM BUILDER */

class FormBuilder {
    constructor(formId, app, blade) {
        if (typeof formId == 'string') {
            this.formId = formId;
            this.fieldModels = this.getFieldModels(formId);
        } else if (formId instanceof Array) {
            var fieldModels = formId;
            this.fieldModels = Promise.resolve(fieldModels);
        }

        this.app = app;
        this.blade = blade;
    }

    getFieldModels(formId) {
        return new FieldDescriptorProvider()
            .getFor(formId)
            .then(fieldDescriptors =>
                Promise.all(
                    fieldDescriptors
                        .map(fieldDescriptor => this.getFieldModel(fieldDescriptor))
                )
            );
    }

    getFieldModel(fieldDescriptor) {
        if (fieldDescriptor.embeddedFormId) {
            var getFieldModels = this.getFieldModels(fieldDescriptor.embeddedFormId);

            if (fieldDescriptor.control) {
                var getFieldControl = FieldControlProvider.getFor(fieldDescriptor);
                return Promise.all([getFieldControl, getFieldModels])
                    .then(result => new FieldModel(fieldDescriptor, result[0], result[1]));
            }

            return getFieldModels
                .then(formFields => new FieldModel(fieldDescriptor, null, formFields));
        } else {
            return FieldControlProvider
                .getFor(fieldDescriptor)
                .then(fieldControl => new FieldModel(fieldDescriptor, fieldControl, null));
        }
    }

    build(target, options) {
        if (!target) {
            target = {};
        }

        return this.fieldModels.then(fieldModels => {
            if (options && 'group' in options) {
                fieldModels = fieldModels.filter(fieldModel => fieldModel.descriptor.group == options.group);
            }

            var form = this.buildForm(fieldModels, target);

            form.element.classList.add('cloudy-ui-form');

            return form;
        });
    }

    buildForm(fieldModels, target) {
        var element = document.createElement('div');

        var fields = fieldModels.map(fieldModel => this.buildField(fieldModel, target));

        fields.forEach(field => element.appendChild(field.element));

        return new Form(this.app, element, target, fields);
    }

    buildField(fieldModel, target) {
        var element = document.createElement(!fieldModel.descriptor.isSortable && fieldModel.descriptor.embeddedFormId ? 'fieldset' : 'div');
        element.classList.add('cloudy-ui-form-field');

        var heading = document.createElement(!fieldModel.descriptor.isSortable && fieldModel.descriptor.embeddedFormId ? 'legend' : 'div');
        heading.classList.add('cloudy-ui-form-field-label');
        heading.innerText = fieldModel.descriptor.label || fieldModel.descriptor.camelCaseId;
        element.appendChild(heading);

        if (fieldModel.descriptor.isSortable) {
            return this.buildSortableField(fieldModel, target, element);
        }

        return this.buildSingularField(fieldModel, target, element);
    }

    buildSingularField(fieldModel, target, element) {
        if (fieldModel.descriptor.embeddedFormId) {
            if (!target[fieldModel.descriptor.camelCaseId]) {
                target[fieldModel.descriptor.camelCaseId] = {};
            }

            var form = this.buildEmbeddedForm(fieldModel, target[fieldModel.descriptor.camelCaseId]);

            element.appendChild(form.element);

            return new Field(fieldModel, element, { form });
        }

        return this.buildSimpleField(fieldModel, target, element);
    }

    buildSimpleField(fieldModel, target, element) {
        element.classList.add('cloudy-ui-simple');

        var control = new fieldModel.controlType(fieldModel, target[fieldModel.descriptor.camelCaseId], this.app, this.blade);

        control.onChange(value => target[fieldModel.descriptor.camelCaseId] = value);

        element.appendChild(control.element);

        return new Field(fieldModel, element, { control });
    }

    buildSortableField(fieldModel, target, element, blade) {
        if (!target[fieldModel.descriptor.camelCaseId]) {
            target[fieldModel.descriptor.camelCaseId] = [];
        }

        var sortable;

        if (fieldModel.descriptor.embeddedFormId) {
            if (fieldModel.descriptor.control) {
                sortable = new fieldModel.controlType(fieldModel, target[fieldModel.descriptor.camelCaseId], this.app, this.blade);
            } else {
                sortable = this.buildSortableEmbeddedForm(fieldModel, target[fieldModel.descriptor.camelCaseId]);
            }
        } else {
            sortable = this.buildSortableSimpleField(fieldModel, target[fieldModel.descriptor.camelCaseId]);
        }

        element.appendChild(sortable.element);

        return new Field(fieldModel, element, { sortable });
    }

    buildSortableEmbeddedForm(fieldModel, target) {
        var createItem =
            index => {
                if (!(index in target)) {
                    target[index] = {};
                }

                var container = document.createElement('cloudy-ui-sortable-item-form');

                var form = this.buildEmbeddedForm(fieldModel, target[index]);

                container.appendChild(form.element);

                return new SortableItem(container, { form, actionContainer: container });
            };

        var sortable = new Sortable(fieldModel, target, createItem);

        sortable.element.classList.add('cloudy-ui-sortable-form');

        return sortable;
    }

    buildSortableSimpleField(fieldModel, target) {
        var createItem =
            index => {
                if (!(index in target)) {
                    target[index] = null;
                }

                var container = document.createElement('cloudy-ui-sortable-item-field');

                var control = new fieldModel.controlType(fieldModel, target[index], this.app, this.blade);

                control.onChange(value => target[index] = value);

                container.appendChild(control.element);

                var field = new Field(fieldModel, container, { control });

                return new SortableItem(container, { field });
            };

        var sortable = new Sortable(fieldModel, target, createItem);

        sortable.element.classList.add('cloudy-ui-sortable-field');

        return sortable;
    }

    buildEmbeddedForm(fieldModel, target) {
        var form = this.buildForm(fieldModel.fields, target);

        form.element.classList.add('cloudy-ui-embedded-form');

        return form;
    }
}

export default FormBuilder;