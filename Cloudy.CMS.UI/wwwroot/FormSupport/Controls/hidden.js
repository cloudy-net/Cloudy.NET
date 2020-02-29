import FieldControl from '../field-control.js';

class HiddenControl extends FieldControl {
    constructor(fieldModel, value, app) {
        super(document.createElement('div'));
    }
}

export default HiddenControl;