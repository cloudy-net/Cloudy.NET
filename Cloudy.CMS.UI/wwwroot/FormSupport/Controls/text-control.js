import FieldControl from '../field-control.js';

class TextControl extends FieldControl {
    static customLabel = true;
    constructor(fieldModel, value, app) {
        var container = document.createElement('div');
        super(container);

        var label = document.createElement('div');
        label.classList.add('poetry-ui-form-field-label');
        label.innerText = fieldModel.descriptor.label || fieldModel.descriptor.camelCaseId;
        container.appendChild(label);

        var input = document.createElement('input');
        input.classList.add('poetry-ui-form-input');
        input.type = 'text';
        input.value = value || null;

        container.append(input);

        input.addEventListener('change', () => this.triggerChange(input.value || null));
        input.addEventListener('keyup', () => this.triggerChange(input.value || null));

        this.onSet(value => input.value = value || null);

        if (value == null) {
            label.classList.add('poetry-ui-enlarge-label');
        }

        input.addEventListener('focus', () => {
            if (input.value == '') {
                label.classList.remove('poetry-ui-enlarge-label');
            }
        });

        input.addEventListener('blur', () => {
            if (input.value == '') {
                label.classList.add('poetry-ui-enlarge-label');
            }
        });
    }
}

export default TextControl;