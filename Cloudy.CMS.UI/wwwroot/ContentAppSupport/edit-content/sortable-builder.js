import Form from '../../FormSupport/form.js';
import Field from '../../FormSupport/field.js';
import Sortable from '../../FormSupport/sortable.js';
import SortableItem from '../../FormSupport/sortable-item.js';
import PopupMenu from '../../PopupMenuSupport/popup-menu.js';
import Button from '../../button.js';
import urlFetcher from '../../url-fetcher.js';

class SortableBuilder {
    build(app, blade, target, fieldModel, eventDispatcher) {
        if (!target[fieldModel.descriptor.camelCaseId]) {
            target[fieldModel.descriptor.camelCaseId] = [];
        }

        var sortable;

        if (fieldModel.descriptor.embeddedFormId) {
            if (fieldModel.descriptor.control) {
                sortable = new fieldModel.controlType(fieldModel, target[fieldModel.descriptor.camelCaseId], app, blade);
            } else {
                sortable = this.buildSortableEmbeddedForm(fieldModel, target[fieldModel.descriptor.camelCaseId], app, blade);
            }
        } else {
            if (fieldModel.descriptor.isPolymorphic) {
                sortable = this.buildSortablePolymorphicField(fieldModel, target[fieldModel.descriptor.camelCaseId], app, blade);
            } else {
                sortable = this.buildSortableSimpleField(fieldModel, target[fieldModel.descriptor.camelCaseId], app, blade, eventDispatcher);
            }
        }

        return sortable;
    }

    buildSortableEmbeddedForm(fieldModel, target, app, blade) {
        var createItem = (value, id) => {
            const container = document.createElement('cloudy-ui-sortable-item-form');
            const form = this.buildEmbeddedForm(fieldModel, value).appendTo(container);
            return new SortableItem(container, id, { form });
        };

        var sortable = new Sortable().setHorizontal();
        sortable.element.classList.add('cloudy-ui-sortable-form');
        let index = 0;
        sortable.addFooter(new Button('Add').onClick(() => sortable.addItem(createItem({}, `new-${index++}`))));

        return sortable;
    }

    buildSortableSimpleField(fieldModel, target, app, blade, eventDispatcher) {
        var createItem = (value, id) => {
            var fieldElement = document.createElement('cloudy-ui-sortable-item-field');
            var fieldControlElement = document.createElement('cloudy-ui-sortable-item-field-control');
            fieldElement.appendChild(fieldControlElement);

            var control = new fieldModel.controlType(fieldModel, value, app, blade).appendTo(fieldControlElement);
            var field = new Field(fieldModel, fieldElement, { control });
            return new SortableItem(fieldElement, id, { field });
        };

        const sortable = new Sortable();
        sortable.element.classList.add('cloudy-ui-sortable-field');

        let index = 0;
        sortable.addFooter(new Button('Add').onClick(() => sortable.add(createItem(null, `new-${index++}`))));

        target.forEach((value, index) => {
            var id = `original-${index}`;
            const item = createItem(value, id);
            sortable.add(item, false);
            item.data.field.data.control.onChange(value => eventDispatcher.triggerChange(fieldModel.descriptor.camelCaseId, { type: 'array.update', value, id }));
        });

        sortable.onAdd(item => {
            item.data.field.data.control.onChange(value =>
                eventDispatcher.triggerChange(fieldModel.descriptor.camelCaseId, { type: 'array.update', value, id: item.id })
            );
            eventDispatcher.triggerChange(fieldModel.descriptor.camelCaseId, { type: 'array.add', value: null, id: item.id });
        });
        sortable.onDelete(index => eventDispatcher.triggerChange(fieldModel.descriptor.camelCaseId, { type: 'array.delete', index }));

        return sortable;
    }

    buildSortablePolymorphicField(fieldModel, target, app, blade) {
        const createItem = (value, id) => {
            var fieldElement = document.createElement('cloudy-ui-sortable-item-field');
            var fieldControlElement = document.createElement('cloudy-ui-sortable-item-field-control');
            fieldElement.appendChild(fieldControlElement);

            var control = new fieldModel.controlType(fieldModel, value, app, blade).appendTo(fieldControlElement);
            var field = new Field(fieldModel, fieldElement, { control });
            return new SortableItem(fieldElement, id, { field });
        };

        const sortable = new Sortable(true).setHorizontal();
        sortable.element.classList.add('cloudy-ui-sortable-field');

        for (let index = 0; index < target.length; index++) {
            sortable.add(createItem(target[index], `original-${index}`), false);
        }

        const button = new Button('Add').onClick(() => menu.toggle());
        const menu = new PopupMenu(button.element);
sortable.addFooter(menu);
        let index = 0;
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
                            sortable.add(createItem(item, `new-${index++}`));
                        });
                    })
                );
            } else {
                menu.addItem(item => item.setDisabled().setText('(no items)'));
            }
        })();

        return sortable;
    }
}

export default new SortableBuilder();