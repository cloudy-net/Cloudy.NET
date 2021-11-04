import FieldControl from '../../FormSupport/field-control.js';
import FormBuilder from './form-builder.js';
import fieldModelBuilder from '../../FormSupport/field-model-builder.js';

/* SELECT CONTROL */

class PolymorphicForm extends FieldControl {
    open = null;

    constructor(fieldModel, value, app, blade, contentId, contentTypeId, path) {
        super(document.createElement('div'));

        var element = document.createElement('fieldset');
        element.classList.add('cloudy-ui-form-field');
        element.style.marginTop = '8px';

        var heading = document.createElement('legend');
        heading.classList.add('cloudy-ui-form-field-label');
        element.appendChild(heading);

        (async () => {
            this.element.append(element);
            heading.innerText = value.name;

            const fieldModels = await fieldModelBuilder.getFieldModels(value.type);
            this.form = new FormBuilder(app, blade)
                .build(contentId, contentTypeId, value, path, fieldModels)
                .onChange((name, value) => console.log(name, contentId, contentTypeId, value, path));
            this.form.element.classList.remove('cloudy-ui-form');
            this.form.element.classList.add('cloudy-ui-embedded-form');
            this.form.appendTo(element);
        })();
    }
}

export default PolymorphicForm;