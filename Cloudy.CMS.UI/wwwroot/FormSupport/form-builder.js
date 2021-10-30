import Form from './form.js';
import Field from './field.js';
import Sortable from './sortable.js';
import SortableItem from './sortable-item.js';
import PopupMenu from '../PopupMenuSupport/popup-menu.js';
import Button from '../button.js';
import urlFetcher from '../url-fetcher.js';
import notificationManager from '../NotificationSupport/notification-manager.js';



/* FORM BUILDER */

class FormBuilder {
    constructor(app, blade) {
        this.app = app;
        this.blade = blade;
    }

    build(target, fieldModels) {
        if (!target) {
            target = {};
        }

        var form = this.buildForm(fieldModels, target);

        form.element.classList.add('cloudy-ui-form');

        return form;
    }

    buildForm(fieldModels, target) {
        try {
            var element = document.createElement('div');

            var fields = fieldModels.map(fieldModel => this.buildField(fieldModel, target));

            fields.forEach(field => element.appendChild(field.element));

            const form = new Form(element, fieldModels, fields);

            for (let field of fields) {
                this.setupEvents(form, field);
            }

            return form;
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not build form --- ${error.message}`));
            throw error;
        }
    }

    setupEvents(form, field) {
        if (field.model.descriptor.isSortable) {
            if (field.model.descriptor.embeddedFormId) {
                if (field.model.descriptor.control) {
                    // sortable embedded form, but control is overridden by UIHint
                    return;
                } else {
                    // sortable embedded form
                    return;
                }
            } else {
                if (field.model.descriptor.isPolymorphic) {
                    // sortable polymorphic form
                    return;
                } else {
                    // sortable simple field
                    field.data.sortable.onAdd(value => form.triggerChange(field.model.descriptor, field.model.descriptor.camelCaseId, { operation: 'add', value }));
                    return;
                }
            }
        }

        if (field.model.descriptor.embeddedFormId) {
            // embedded form
            return;
        }

        // singular
        field.data.control.onChange(value => form.triggerChange(field.model.descriptor, field.model.descriptor.camelCaseId, value));
    }

    buildField(fieldModel, target) {
        var element = document.createElement(!fieldModel.descriptor.isSortable && fieldModel.descriptor.embeddedFormId ? 'fieldset' : 'div');
        element.classList.add('cloudy-ui-form-field');

        var heading = document.createElement(!fieldModel.descriptor.isSortable && fieldModel.descriptor.embeddedFormId ? 'legend' : 'div');
        heading.classList.add('cloudy-ui-form-field-label');
        heading.innerText = fieldModel.descriptor.label || fieldModel.descriptor.camelCaseId;
        element.appendChild(heading);

        if (fieldModel.descriptor.isSortable) {
            return this.buildSortableField(fieldModel, target, element);
        }

        return this.buildSingularField(fieldModel, target, element);
    }

    buildSingularField(fieldModel, target, element) {
        if (fieldModel.descriptor.embeddedFormId) {
            if (!target[fieldModel.descriptor.camelCaseId]) {
                target[fieldModel.descriptor.camelCaseId] = {};
            }

            var form = this.buildEmbeddedForm(fieldModel, target[fieldModel.descriptor.camelCaseId]);

            element.appendChild(form.element);

            return new Field(fieldModel, element, { form });
        }

        return this.buildSimpleField(fieldModel, target, element);
    }

    buildSimpleField(fieldModel, target, element) {
        element.classList.add('cloudy-ui-simple');
        
        var control = new fieldModel.controlType(fieldModel, target[fieldModel.descriptor.camelCaseId], this.app, this.blade, target[fieldModel.descriptor.camelCaseId]);

        element.appendChild(control.element);

        return new Field(fieldModel, element, { control });
    }

    buildSortableField(fieldModel, target, element) {
        if (!target[fieldModel.descriptor.camelCaseId]) {
            target[fieldModel.descriptor.camelCaseId] = [];
        }

        var sortable;

        if (fieldModel.descriptor.embeddedFormId) {
            if (fieldModel.descriptor.control) {
                sortable = new fieldModel.controlType(fieldModel, target[fieldModel.descriptor.camelCaseId], this.app, this.blade);
            } else {
                sortable = this.buildSortableEmbeddedForm(fieldModel, target[fieldModel.descriptor.camelCaseId]);
            }
        } else {
            if (fieldModel.descriptor.isPolymorphic) {
                sortable = this.buildSortablePolymorphicField(fieldModel, target[fieldModel.descriptor.camelCaseId]);
            } else {
                sortable = this.buildSortableSimpleField(fieldModel, target[fieldModel.descriptor.camelCaseId]);
            }
        }

        element.appendChild(sortable.element);

        return new Field(fieldModel, element, { sortable });
    }

    buildSortableEmbeddedForm(fieldModel, target) {
        var createItem =
            index => {
                if (!(index in target)) {
                    target[index] = {};
                }

                var container = document.createElement('cloudy-ui-sortable-item-form');

                var form = this.buildEmbeddedForm(fieldModel, target[index]);

                container.appendChild(form.element);

                return new SortableItem(container, { form, actionContainer: container });
            };

        var sortable = new Sortable().setHorizontal();
        sortable.element.classList.add('cloudy-ui-sortable-form');

        var buttonContainer = document.createElement('cloudy-ui-sortable-buttons');
        new Button('Add')
            .onClick(() => {
                var item = this.createItem(-1);
                this.addItem(item);
                this.triggerAdd(item);
            })
            .appendTo(buttonContainer);
        addButton.element.style.marginTop = '8px';
        sortable.addFooter(buttonContainer);

        return sortable;
    }

    buildSortableSimpleField(fieldModel, target) {
        var createItem =
            index => {
                if (!(index in target)) {
                    target[index] = null;
                }

                var fieldElement = document.createElement('cloudy-ui-sortable-item-field');
                var fieldControlElement = document.createElement('cloudy-ui-sortable-item-field-control');
                fieldElement.appendChild(fieldControlElement);

                var control = new fieldModel.controlType(fieldModel, target[index], this.app, this.blade);

                fieldControlElement.appendChild(control.element);

                var field = new Field(fieldModel, fieldElement, { control });

                return new SortableItem(fieldElement, { field });
            };

        var sortable = new Sortable();
        sortable.element.classList.add('cloudy-ui-sortable-field');

        var buttonContainer = document.createElement('cloudy-ui-sortable-buttons');
        new Button('Add')
            .onClick(() => {
                var item = createItem(-1);
                sortable.addItem(item);
                sortable.triggerAdd(item);
            })
            .appendTo(buttonContainer);
        sortable.addFooter(buttonContainer);

        return sortable;
    }

    buildSortablePolymorphicField(fieldModel, target) {
        const createItem = value => {
            var fieldElement = document.createElement('cloudy-ui-sortable-item-field');
            var fieldControlElement = document.createElement('cloudy-ui-sortable-item-field-control');
            fieldElement.appendChild(fieldControlElement);

            var control = new fieldModel.controlType(fieldModel, value, this.app, this.blade).appendTo(fieldControlElement);
            var field = new Field(fieldModel, fieldElement, { control });

            return new SortableItem(fieldElement, { field });
        };

        const sortable = new Sortable(true).setHorizontal();
        sortable.element.classList.add('cloudy-ui-sortable-field');

        for (let index = 0; index < target.length; index++) {
            sortable.addItem(createItem(target[index]));
        }

        const buttonContainer = document.createElement('cloudy-ui-sortable-buttons');
        buttonContainer.style.marginTop = '8px';
        const button = new Button('Add').onClick(() => menu.toggle());
        const menu = new PopupMenu(button.element);
        menu.appendTo(buttonContainer);
        sortable.addFooter(buttonContainer);

        (async () => {
            const types = await urlFetcher.fetch(`PolymorphicForm/GetOptions?${fieldModel.descriptor.polymorphicCandidates.map((t, i) => `types[${i}]=${t}`).join('&')}`, {
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json'
                }
            }, `Could not get form types for ${fieldModel.descriptor.polymorphicCandidates.join(', ')}`);

            if (types.length) {
                types.forEach(item =>
                    menu.addItem(listItem => {
                        listItem.setText(item.name);
                        listItem.onClick(() => {
                            sortable.addItem(createItem(item));
                        });
                    })
                );
            } else {
                menu.addItem(item => item.setDisabled().setText('(no items)'));
            }
        })();

        return sortable;
    }

    buildEmbeddedForm(fieldModel, target) {
        var form = this.buildForm(fieldModel.fields, target);

        form.element.classList.add('cloudy-ui-embedded-form');

        return form;
    }
}

export default FormBuilder;