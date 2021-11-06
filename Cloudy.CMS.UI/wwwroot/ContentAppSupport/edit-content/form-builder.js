import Form from '../../FormSupport/form.js';
import Field from '../../FormSupport/field.js';
import notificationManager from '../../NotificationSupport/notification-manager.js';
import sortableBuilder from './sortable-builder.js';
import FormEventDispatcher from '../../FormSupport/form-event-dispatcher.js';
import changeTracker from '../utils/change-tracker.js';

class FormBuilder {
    constructor(app, blade) {
        this.app = app;
        this.blade = blade;
    }

    async build(contentId, contentTypeId, content, path, fieldModels) {
        try {
            const element = document.createElement('div');
            const eventDispatcher = new FormEventDispatcher();
            const fields = [];

            for (const fieldModel of fieldModels) {
                const field = await this.buildField(contentId, contentTypeId, path, fieldModel, content[fieldModel.descriptor.camelCaseId], eventDispatcher);

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

    async buildField(contentId, contentTypeId, path, fieldModel, value, eventDispatcher) {
        path = [...path, fieldModel.descriptor.camelCaseId];

        if (fieldModel.descriptor.isSortable) {
            return await this.buildSortable(contentId, contentTypeId, path, fieldModel, value, eventDispatcher)
        }

        if (fieldModel.descriptor.embeddedFormId) {
            return this.buildEmbeddedForm(fieldModel, value);
        }

        return this.buildSimpleField(contentId, contentTypeId, path, fieldModel, value, eventDispatcher);
    }

    async buildSortable(contentId, contentTypeId, path, fieldModel, value, eventDispatcher) {
        const element = document.createElement(!fieldModel.descriptor.isSortable && fieldModel.descriptor.embeddedFormId ? 'fieldset' : 'div');
        element.classList.add('cloudy-ui-form-field');

        const heading = document.createElement(!fieldModel.descriptor.isSortable && fieldModel.descriptor.embeddedFormId ? 'legend' : 'div');
        heading.classList.add('cloudy-ui-form-field-label');
        heading.innerText = fieldModel.descriptor.label || fieldModel.descriptor.camelCaseId;
        element.appendChild(heading);

        const sortable = await sortableBuilder.build(this.app, this.blade, contentId, contentTypeId, path, fieldModel, value, eventDispatcher);

        element.appendChild(sortable.element);

        return new Field(fieldModel, element, { sortable });
    }

    buildSimpleField(contentId, contentTypeId, path, fieldModel, initialValue, eventDispatcher) {
        const element = document.createElement(!fieldModel.descriptor.isSortable && fieldModel.descriptor.embeddedFormId ? 'fieldset' : 'div');
        element.classList.add('cloudy-ui-form-field');
        element.classList.add('cloudy-ui-simple');

        const heading = document.createElement(!fieldModel.descriptor.isSortable && fieldModel.descriptor.embeddedFormId ? 'legend' : 'div');
        heading.classList.add('cloudy-ui-form-field-label');
        heading.innerText = fieldModel.descriptor.label || fieldModel.descriptor.camelCaseId;
        element.appendChild(heading);

        const pendingValue = changeTracker.getPendingValue(contentId, contentTypeId, path, initialValue);

        const control = new fieldModel.controlType(fieldModel, pendingValue, this.app, this.blade)
            .appendTo(element)
            .onChange(value => eventDispatcher.triggerChange(path, { fieldType: 'simple', type: 'set', initialValue, value }));
            
        return new Field(fieldModel, element, { control });
    }

    buildEmbeddedForm(fieldModel, value) {
        const element = document.createElement('fieldset');
        element.classList.add('cloudy-ui-form-field');

        const heading = document.createElement('legend');
        heading.classList.add('cloudy-ui-form-field-label');
        heading.innerText = fieldModel.descriptor.label || fieldModel.descriptor.camelCaseId;
        element.appendChild(heading);

        const form = this.build(fieldModel.fields, value).appendTo(element);

        form.element.classList.remove('cloudy-ui-form');
        form.element.classList.add('cloudy-ui-embedded-form');

        return new Field(fieldModel, element, { form });
    }
}

export default FormBuilder;