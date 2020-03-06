import FieldControl from '../field-control.js';

class TextControl extends FieldControl {
    constructor(fieldModel, value, app, blade) {
        var container = document.createElement('div');
        super(container);

        var input = document.createElement('input');
        input.classList.add('cloudy-ui-form-input');

        if (fieldModel.descriptor.control.uiHint == 'password') {
            input.type = 'password';
        } else {
            input.type = 'text';
        }
        input.value = value || null;
        input.name = fieldModel.descriptor.camelCaseId;

        container.append(input);

        input.addEventListener('change', () => this.triggerChange(input.value || null));
        input.addEventListener('keyup', () => this.triggerChange(input.value || null));

        this.onSet(value => input.value = value || null);
    }
}

export default TextControl;