import FieldControl from '../field-control.js';

class TextareaControl extends FieldControl {
    constructor(fieldModel, value, app, blade) {
        var container = document.createElement('div');
        super(container);

        var input = document.createElement('textarea');
        input.classList.add('cloudy-ui-form-input');
        input.rows = fieldModel.descriptor.control.parameters.options && fieldModel.descriptor.control.parameters.options.rows ? fieldModel.descriptor.control.parameters.options.rows.value : 8;
        input.value = value || null;
        container.append(input);

        input.addEventListener('change', () => this.triggerChange(input.value || null));
        input.addEventListener('keyup', () => this.triggerChange(input.value || null));

        this.onSet(value => input.value = value || null);
    }
}

export default TextareaControl;