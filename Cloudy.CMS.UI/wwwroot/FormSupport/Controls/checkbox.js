import FieldControl from '../field-control.js';

class CheckboxControl extends FieldControl {
    constructor(fieldModel, value, app) {
        var input = document.createElement('input');
        input.classList.add('poetry-ui-form-input');
        input.type = 'checkbox';
        input.checked = value || null;

        super(input);

        input.addEventListener('change', () => this.triggerChange(input.checked));

        this.onSet(value => input.checked = value || null);
    }
}

export default CheckboxControl;