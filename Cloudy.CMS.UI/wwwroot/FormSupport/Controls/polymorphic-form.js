import FieldControl from '../field-control.js';
import Blade from '../../blade.js';
import Button from '../../button.js';
import List from '../../ListSupport/list.js';
import ListItem from '../../ListSupport/list-item.js';
import contentTypeProvider from '../../ContentAppSupport/content-type-provider.js';
import FormBuilder from '../form-builder.js';



/* SELECT CONTROL */

class PolymorphicForm extends FieldControl {
    open = null;

    constructor(fieldModel, value, app, blade) {
        super(document.createElement('div'));

        if (!value) {
            value = {};
        }

        this.form = null;

        var element = document.createElement('fieldset');
        element.classList.add('cloudy-ui-form-field');
        element.style.marginTop = '8px';

        var heading = document.createElement('legend');
        heading.classList.add('cloudy-ui-form-field-label');
        element.appendChild(heading);

        var update = async () => {

            this.element.append(element);

            this.form = await new FormBuilder(value.formId, app, blade).build(value.value, {});
            this.form.element.classList.remove('cloudy-ui-form');
            this.form.element.classList.add('cloudy-ui-embedded-form');
            this.form.appendTo(element);
        };

        this.open = () => {
            var list = new ListItemsBlade(app, fieldModel)
                .onSelect(item => {
                    value.formId = item.formId;
                    value.value = {};
                    heading.innerText = item.name;
                    update();
                    this.triggerChange(value);
                    app.removeBlade(list);
                });

            app.addBladeAfter(list, blade);
        };

        this.onSet(value => {
            update(value);
        });
    }
}



/* LIST  BLADE */

class ListItemsBlade extends Blade {
    onSelectCallbacks = [];

    constructor(app, fieldModel) {
        super();

        this.app = app;
        this.name = fieldModel.descriptor.singularLabel;
        this.types = fieldModel.descriptor.control.types;
    }

    async open() {
        this.setTitle(`Add ${this.name.substr(0, 1).toLowerCase()}${this.name.substr(1)}`);

        var list = new List();
        this.setContent(list);


        try {
            var response = await fetch(`PolymorphicFormControl/GetOptions?${this.types.map((t, i) => `types[${i}]=${t}`).join('&')}`, {
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                var text = await response.text();

                if (text) {
                    throw new Error(text.split('\n')[0]);
                } else {
                    text = response.statusText;
                }

                throw new Error(`${response.status} (${text})`);
            }

            var items = await response.json();
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not get item ${value} of type ${type} for select control ${provider} (${error.message})`));
        }

        if (!items.length) {    
            var listItem = new ListItem();
            listItem.setDisabled();
            listItem.setText('(no items)');
            list.addItem(listItem);
            return;
        }

        items.forEach(item =>
            list.addItem(listItem => {
                listItem.setText(item.name);
                listItem.onClick(() => {
                    listItem.setActive();
                    this.onSelectCallbacks.forEach(callback => callback.apply(this, [item]));
                });
            })
        );
    }

    onSelect(callback) {
        this.onSelectCallbacks.push(callback);

        return this;
    }
}

export default PolymorphicForm;