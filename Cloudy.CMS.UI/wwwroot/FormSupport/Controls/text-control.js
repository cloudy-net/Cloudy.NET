import FieldControl from '../field-control.js';

class TextControl extends FieldControl {
    static customLabel = true;
    constructor(fieldModel, value, app) {
        var container = document.createElement('div');
        super(container);

        var input = document.createElement('input');
        input.classList.add('poetry-ui-form-input');
        input.type = 'text';
        input.value = value || null;
        input.name = fieldModel.descriptor.camelCaseId;

        container.append(input);

        input.addEventListener('change', () => this.triggerChange(input.value || null));
        input.addEventListener('keyup', () => this.triggerChange(input.value || null));

        this.onSet(value => input.value = value || null);

        if (fieldModel.descriptor.isSortable) {
            return;
        }

        var label = document.createElement('div');
        label.classList.add('poetry-ui-form-field-label');
        label.classList.add('poetry-ui-enlargeable-label');
        label.innerText = fieldModel.descriptor.label || fieldModel.descriptor.camelCaseId;
        container.prepend(label);

        if (value == null) {
            label.classList.add('poetry-ui-enlarge-label');
        }

        input.addEventListener('focus', () => {
            label.classList.add('poetry-ui-focus');

            if (input.value == '') {
                label.classList.remove('poetry-ui-enlarge-label');
            }
        });

        input.addEventListener('blur', () => {
            label.classList.remove('poetry-ui-focus');

            if (input.value == '') {
                label.classList.add('poetry-ui-enlarge-label');
            }
        });
    }
}

export default TextControl;