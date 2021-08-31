import FieldControl from '../field-control.js';

class TextControl extends FieldControl {
    constructor(fieldModel, value, app, blade, originalValue) {
        var container = document.createElement('div');
        super(container);

        this.contentId = blade?.content?.id;
        this.contentTypeId = blade?.contentType?.id;
        this.changeTracker = app.changeTracker;
        this.path = fieldModel.descriptor.camelCaseId;
        this.name = this.changeTracker?.buildControlName(this.contentTypeId, this.contentId, fieldModel.descriptor.camelCaseId);
        this.backupValue = originalValue || value;
      
        var input = document.createElement('input');
        input.classList.add('cloudy-ui-form-input');

        if (fieldModel.descriptor.control.uiHint == 'password') {
            input.type = 'password';
        } else {
            input.type = 'text';
        }
        input.value = value || null;
        input.name = this.name;
        container.append(input);
        input.addEventListener('change', () => this.triggerChange(input.value || null));
        input.addEventListener('keyup', () => this.triggerChange(input.value || null));

        this.onSet(value => input.value = value || null);
    }
}

export default TextControl;