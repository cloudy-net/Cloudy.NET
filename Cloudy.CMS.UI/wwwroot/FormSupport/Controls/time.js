import FieldControl from '../field-control.js';

class TimeControl extends FieldControl {
    constructor(fieldModel, value, app, blade) {
        var input = document.createElement('input');
        input.classList.add('cloudy-ui-form-input');
        input.type = 'time';
        input.value = value || null;

        super(input);
        
        input.addEventListener('change', () => this.triggerChange(input.value || null));
        input.addEventListener('keyup', () => this.triggerChange(input.value || null));

        this.onSet(value => input.value = value || null);
    }
}

export default TimeControl;