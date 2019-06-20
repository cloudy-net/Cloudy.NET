import Form from './form.js';
import Field from './field.js';
import FieldModel from './field-model.js';
import FieldDescriptorProvider from './field-descriptor-provider.js';
import FieldControlProvider from './field-control-provider.js';
import Sortable from './sortable.js';
import SortableItem from './sortable-item.js';
import ContextMenu from '../../Poetry.UI.ContextMenuSupport/Scripts/context-menu.js';



/* FORM BUILDER */

class FormBuilder {
    constructor(formId, app) {
        this.formId = formId;
        this.app = app;

        this.fieldModels = this.getFieldModels(formId);
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
        if (fieldDescriptor.EmbeddedFormId) {
            var getFieldModels = this.getFieldModels(fieldDescriptor.EmbeddedFormId);

            if (fieldDescriptor.Control) {
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
                fieldModels = fieldModels.filter(fieldModel => fieldModel.descriptor.Group == options.group);
            }

            var form = this.buildForm(fieldModels, target);

            form.element.classList.add('poetry-ui-form');

            var headingContainer = document.createElement('div');
            headingContainer.classList.add('poetry-ui-form-heading-outer');
            form.element.insertBefore(headingContainer, form.element.firstChild);

            var heading = document.createElement('h2');
            heading.classList.add('poetry-ui-form-heading');
            headingContainer.appendChild(heading);

            new ContextMenu()
                .addItem(item => item.setText('Copy').onClick(() => form.copy()))
                .addItem(item => item.setText('Paste').onClick(() => form.paste()))
                .appendTo(headingContainer);

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
        var element = document.createElement(fieldModel.descriptor.IsSortable || fieldModel.descriptor.EmbeddedFormId ? 'fieldset' : 'label');
        element.classList.add('poetry-ui-form-field');

        var heading = document.createElement(fieldModel.descriptor.IsSortable || fieldModel.descriptor.EmbeddedFormId ? 'legend' : 'div');
        heading.classList.add('poetry-ui-form-field-label');
        heading.innerText = fieldModel.descriptor.Label || fieldModel.descriptor.Id;
        element.appendChild(heading);

        if (fieldModel.descriptor.IsSortable) {
            return this.buildSortableField(fieldModel, target, element);
        }

        return this.buildSingularField(fieldModel, target, element);
    }

    buildSingularField(fieldModel, target, element) {
        if (fieldModel.descriptor.EmbeddedFormId) {
            if (!target[fieldModel.descriptor.Id]) {
                target[fieldModel.descriptor.Id] = {};
            }

            var form = this.buildEmbeddedForm(fieldModel, target[fieldModel.descriptor.Id]);

            element.appendChild(form.element);

            return new Field(fieldModel, element, { form });
        }

        return this.buildSimpleField(fieldModel, target, element);
    }

    buildSimpleField(fieldModel, target, element) {
        element.classList.add('poetry-ui-simple');

        var control = new fieldModel.controlType(fieldModel, target[fieldModel.descriptor.Id], this.app);

        control.onChange(value => target[fieldModel.descriptor.Id] = value);

        element.appendChild(control.element);

        return new Field(fieldModel, element, { control });
    }

    buildSortableField(fieldModel, target, element) {
        if (!target[fieldModel.descriptor.Id]) {
            target[fieldModel.descriptor.Id] = [];
        }

        var sortable;

        if (fieldModel.descriptor.EmbeddedFormId) {
            if (fieldModel.descriptor.Control) {
                sortable = new fieldModel.controlType(fieldModel, target[fieldModel.descriptor.Id], this.app);
            } else {
                sortable = this.buildSortableEmbeddedForm(fieldModel, target[fieldModel.descriptor.Id]);
            }
        } else {
            sortable = this.buildSortableSimpleField(fieldModel, target[fieldModel.descriptor.Id]);
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

                var container = document.createElement('poetry-ui-sortable-item-form');

                var form = this.buildEmbeddedForm(fieldModel, target[index]);

                container.appendChild(form.element);

                return new SortableItem(container, { form });
            };

        var sortable = new Sortable(fieldModel, target, createItem);

        sortable.element.classList.add('poetry-ui-sortable-form');

        return sortable;
    }

    buildSortableSimpleField(fieldModel, target) {
        var createItem =
            index => {
                if (!(index in target)) {
                    target[index] = null;
                }

                var container = document.createElement('poetry-ui-sortable-item-field');

                var control = new fieldModel.controlType(fieldModel, target[index], this.app);

                control.onChange(value => target[index] = value);

                container.appendChild(control.element);

                var field = new Field(fieldModel, container, { control });

                return new SortableItem(container, { field });
            };

        var sortable = new Sortable(fieldModel, target, createItem);

        sortable.element.classList.add('poetry-ui-sortable-field');

        return sortable;
    }

    buildEmbeddedForm(fieldModel, target) {
        var form = this.buildForm(fieldModel.fields, target);

        form.element.classList.add('poetry-ui-embedded-form');

        return form;
    }
}

export default FormBuilder;