import FieldControl from '../../FormSupport/field-control.js';
import FormBuilder from './form-builder.js';
import fieldModelBuilder from '../../FormSupport/field-model-builder.js';

class PolymorphicForm extends FieldControl {
    open = null;

    constructor(fieldModel, value, app, blade, contentId, contentTypeId, path) {
        super(document.createElement('div'));
    }
}

export default PolymorphicForm;