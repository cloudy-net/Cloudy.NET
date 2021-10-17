import FieldControl from '../field-control.js';
import Blade from '../../blade.js';
import List from '../../ListSupport/list.js';
import ListItem from '../../ListSupport/list-item.js';
import FormBuilder from '../form-builder.js';
import fieldModelBuilder from '../field-model-builder.js';
import urlFetcher from '../../url-fetcher.js';

/* SELECT CONTROL */

class PolymorphicForm extends FieldControl {
    open = null;

    constructor(fieldModel, value, app, blade) {
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
            var onChangeCallback = (name, value, originalValue) => console.log(name, value, originalValue);
            this.form = new FormBuilder(app, blade).build({}, fieldModels, onChangeCallback);
            this.form.element.classList.remove('cloudy-ui-form');
            this.form.element.classList.add('cloudy-ui-embedded-form');
            this.form.appendTo(element);
        })();
    }
}

export default PolymorphicForm;