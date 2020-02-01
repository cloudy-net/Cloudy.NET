import FieldControl from '../field-control.js';

class TextControl extends FieldControl {
    constructor(fieldModel, value, app) {
        var input = document.createElement('input');
        input.classList.add('poetry-ui-form-input');
        input.type = 'text';
        input.value = value || null;

        super(input);

        input.addEventListener('change', () => this.triggerChange(input.value || null));
        input.addEventListener('keyup', () => this.triggerChange(input.value || null));

        this.onSet(value => input.value = value || null);

        if (value == null) {
            this.setEnlargeLabel(true);
        }

        input.addEventListener('focus', () => {
            if (input.value == '') {
                this.setEnlargeLabel(false);
            }
        });

        input.addEventListener('blur', () => {
            if (input.value == '') {
                this.setEnlargeLabel(true);
            }
        });
    }
}

export default TextControl;