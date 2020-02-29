import FieldControl from '../field-control.js';

class HiddenControl extends FieldControl {
    static customLabel = true;
    constructor(fieldModel, value, app) {
        super(document.createElement('div'));
    }
}

export default HiddenControl;