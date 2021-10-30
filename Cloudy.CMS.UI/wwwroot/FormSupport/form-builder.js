import Form from './form.js';
import Field from './field.js';
import Sortable from './sortable.js';
import SortableItem from './sortable-item.js';
import PopupMenu from '../PopupMenuSupport/popup-menu.js';
import Button from '../button.js';
import urlFetcher from '../url-fetcher.js';
import notificationManager from '../NotificationSupport/notification-manager.js';
import sortableBuilder from './sortable-builder.js';



/* FORM BUILDER */

class FormBuilder {
    constructor(app, blade) {
        this.app = app;
        this.blade = blade;
    }

    build(target, fieldModels) {
        if (!target) {
            target = {};
        }

        var form = this.buildForm(fieldModels, target);

        form.element.classList.add('cloudy-ui-form');

        return form;
    }

    buildForm(fieldModels, target) {
        try {
            var element = document.createElement('div');

            var fields = fieldModels.map(fieldModel => this.buildField(fieldModel, target));

            fields.forEach(field => element.appendChild(field.element));

            return new Form(element, fieldModels, fields);
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not build form --- ${error.message}`));
            throw error;
        }
    }

    buildField(fieldModel, target) {
        var element = document.createElement(!fieldModel.descriptor.isSortable && fieldModel.descriptor.embeddedFormId ? 'fieldset' : 'div');
        element.classList.add('cloudy-ui-form-field');

        var heading = document.createElement(!fieldModel.descriptor.isSortable && fieldModel.descriptor.embeddedFormId ? 'legend' : 'div');
        heading.classList.add('cloudy-ui-form-field-label');
        heading.innerText = fieldModel.descriptor.label || fieldModel.descriptor.camelCaseId;
        element.appendChild(heading);

        if (fieldModel.descriptor.isSortable) {
            return sortableBuilder.build(this.app, this.blade, target, fieldModel);
        }

        if (fieldModel.descriptor.embeddedFormId) {
            return this.buildEmbeddedForm(fieldModel, target[fieldModel.descriptor.camelCaseId], element);
        }

        return this.buildSimpleField(fieldModel, target[fieldModel.descriptor.camelCaseId], element);
    }

    buildSimpleField(fieldModel, value, element) {
        element.classList.add('cloudy-ui-simple');

        var control = new fieldModel.controlType(fieldModel, value, this.app, this.blade).appendTo(element);

        control.onChange(value => form.triggerChange(fieldModel.descriptor, fieldModel.descriptor.camelCaseId, value));

        return new Field(fieldModel, element, { control });
    }

    buildEmbeddedForm(fieldModel, value, element) {
        var form = this.buildForm(fieldModel.fields, value).appendTo(element);

        form.element.classList.add('cloudy-ui-embedded-form');

        return new Field(fieldModel, element, { form });
    }
}

export default FormBuilder;