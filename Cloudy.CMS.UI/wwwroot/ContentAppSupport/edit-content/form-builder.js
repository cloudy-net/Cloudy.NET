import Form from '../../FormSupport/form.js';
import Field from '../../FormSupport/field.js';
import notificationManager from '../../NotificationSupport/notification-manager.js';
import sortableBuilder from './sortable-builder.js';
import FormEventDispatcher from '../../FormSupport/form-event-dispatcher.js';

class FormBuilder {
    constructor(app, blade) {
        this.app = app;
        this.blade = blade;
    }

    build(target, fieldModels) {
        try {
            const element = document.createElement('div');
            const eventDispatcher = new FormEventDispatcher();
            const fields = [];

            for (const fieldModel of fieldModels) {
                const field = this.buildField(fieldModel, target, eventDispatcher);

                element.appendChild(field.element);

                fields.push(field);
            }

            element.classList.add('cloudy-ui-form');

            return new Form(element, fieldModels, fields, eventDispatcher);
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not build form --- ${error.message}`));
            throw error;
        }
    }

    buildField(fieldModel, target, eventDispatcher) {
        var element = document.createElement(!fieldModel.descriptor.isSortable && fieldModel.descriptor.embeddedFormId ? 'fieldset' : 'div');
        element.classList.add('cloudy-ui-form-field');

        var heading = document.createElement(!fieldModel.descriptor.isSortable && fieldModel.descriptor.embeddedFormId ? 'legend' : 'div');
        heading.classList.add('cloudy-ui-form-field-label');
        heading.innerText = fieldModel.descriptor.label || fieldModel.descriptor.camelCaseId;
        element.appendChild(heading);

        if (fieldModel.descriptor.isSortable) {
            return sortableBuilder.build(this.app, this.blade, target, fieldModel, eventDispatcher);
        }

        if (fieldModel.descriptor.embeddedFormId) {
            return this.buildEmbeddedForm(fieldModel, target[fieldModel.descriptor.camelCaseId], element);
        }

        return this.buildSimpleField(fieldModel, target[fieldModel.descriptor.camelCaseId], element, eventDispatcher);
    }

    buildSimpleField(fieldModel, value, element, eventDispatcher) {
        element.classList.add('cloudy-ui-simple');

        var control = new fieldModel.controlType(fieldModel, value, this.app, this.blade).appendTo(element);

        control.onChange(value => eventDispatcher.triggerChange(fieldModel.descriptor.camelCaseId, { type: 'change', value }));

        return new Field(fieldModel, element, { control });
    }

    buildEmbeddedForm(fieldModel, value, element) {
        var form = this.build(fieldModel.fields, value).appendTo(element);

        form.element.classList.remove('cloudy-ui-form');
        form.element.classList.add('cloudy-ui-embedded-form');

        return new Field(fieldModel, element, { form });
    }
}

export default FormBuilder;