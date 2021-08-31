import FieldControl from '../field-control.js';

class CheckboxControl extends FieldControl {
    constructor(fieldModel, value, app, blade) {
        var container = document.createElement('cloudy-ui-checkbox-container');
        super(container);

        this.contentId = blade?.content?.id;
        this.contentTypeId = blade?.contentType?.id;
        this.changeTracker = app.changeTracker;
        this.path = fieldModel.descriptor.camelCaseId;
        this.name = this.changeTracker?.buildControlName(this.contentTypeId, this.contentId, fieldModel.descriptor.camelCaseId);
        this.backupValue = originalValue || value;
        
        var input = document.createElement('input');
        input.classList.add('cloudy-ui-checkbox');
        input.type = 'checkbox';
        input.checked = value || null;
        container.append(input);

        var graphicalCheckbox = document.createElement('span');
        graphicalCheckbox.classList.add('cloudy-ui-graphical-checkbox');
        container.append(graphicalCheckbox);

        var label = document.createElement('cloudy-ui-checkbox-label');
        label.innerText = fieldModel.descriptor.label || fieldModel.descriptor.camelCaseId;
        container.append(label);

        input.addEventListener('change', () => this.triggerChange(input.checked));

        this.onSet(value => input.checked = value || null);
    }
}

export default CheckboxControl;