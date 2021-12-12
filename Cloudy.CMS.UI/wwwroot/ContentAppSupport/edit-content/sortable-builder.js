import Field from '../../FormSupport/field.js';
import Sortable from '../../FormSupport/sortable.js';
import SortableItem from '../../FormSupport/sortable-item.js';
import PopupMenu from '../../components/popup-menu/popup-menu.js';
import Button from '../../components/button/button.js';
import urlFetcher from '../../util/url-fetcher.js';
import changeTracker from '../utils/change-tracker.js';
import arrayEquals from '../utils/array-equals.js';
import fieldModelBuilder from '../../FormSupport/field-model-builder.js';
import FormBuilder from './form-builder.js';
import SortableMenu from '../../FormSupport/sortable-menu.js';

class SortableBuilder {
    async build(app, blade, contentId, contentTypeId, path, fieldModel, value, eventDispatcher) {
        if (!value) {
            value = [];
        }

        if (fieldModel.descriptor.embeddedFormId) {
            if (fieldModel.descriptor.control) {
                return new fieldModel.controlType(app, blade, contentId, contentTypeId, path, fieldModel, value, eventDispatcher);
            } else {
                return this.buildSortableEmbeddedForm(app, blade, contentId, contentTypeId, path, fieldModel, value, eventDispatcher);
            }
        } else {
            if (fieldModel.descriptor.isPolymorphic) {
                return await this.buildSortablePolymorphicField(app, blade, contentId, contentTypeId, path, fieldModel, value, eventDispatcher);
            } else {
                return this.buildSortableSimpleField(app, blade, contentId, contentTypeId, path, fieldModel, value, eventDispatcher);
            }
        }
    }

    buildSortableEmbeddedForm(app, blade, contentId, contentTypeId, path, fieldModel, value, eventDispatcher) {
        var createItem = (value, id) => {
            const container = document.createElement('cloudy-ui-sortable-item-form');
            const form = this.buildEmbeddedForm(fieldModel, value).appendTo(container);
            return new SortableItem(container, id, { form });
        };

        var sortable = new Sortable();
        sortable.element.classList.add('cloudy-ui-sortable-form');
        let index = 0;
        sortable.addFooter(new Button('Add').onClick(() => sortable.addItem(createItem({}, `new-${index++}`))));

        return sortable;
    }

    buildSortableSimpleField(app, blade, contentId, contentTypeId, path, fieldModel, value, eventDispatcher) {
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

        value.forEach((value, index) => {
            var id = `original-${index}`;
            const item = createItem(value, id);
            sortable.add(item, false);
            item.data.field.data.control.onChange(value => eventDispatcher.triggerChange(fieldModel.descriptor.id, { type: 'array.update', value, id }));
        });

        sortable.onAdd(item => {
            item.data.field.data.control.onChange(value =>
                eventDispatcher.triggerChange(fieldModel.descriptor.id, { type: 'array.update', value, id: item.id })
            );
            eventDispatcher.triggerChange(fieldModel.descriptor.id, { type: 'array.add', value: null, id: item.id });
        });
        sortable.onDelete(index => eventDispatcher.triggerChange(fieldModel.descriptor.id, { type: 'array.delete', index }));

        return sortable;
    }

    async buildSortablePolymorphicField(app, blade, contentId, contentTypeId, path, fieldModel, value, eventDispatcher) {
        const types = await urlFetcher.fetch(
            `PolymorphicForm/GetOptions?${fieldModel.descriptor.polymorphicCandidates.map((t, i) => `types[${i}]=${t}`).join('&')}`,
            { credentials: 'include', headers: { 'Content-Type': 'application/json' } },
            `Could not get form types for ${fieldModel.descriptor.polymorphicCandidates.join(', ')}`
        );

        const createItem = async (type, value, id) => {
            const fieldElement = document.createElement('cloudy-ui-sortable-item-field');
            const fieldControlElement = document.createElement('cloudy-ui-sortable-item-field-control');
            fieldElement.appendChild(fieldControlElement);

            const fieldset = document.createElement('fieldset');
            fieldset.classList.add('cloudy-ui-form-field');
            fieldset.style.marginTop = '8px';
            fieldControlElement.appendChild(fieldset);

            const legend = document.createElement('legend');
            legend.classList.add('cloudy-ui-form-field-label');
            legend.innerText = types.find(t => t.type == type).name;
            fieldset.appendChild(legend);

            const data = { type };

            const fieldModels = await fieldModelBuilder.getFieldModels(type);

            const form = (await new FormBuilder(app, blade).build(contentId, contentTypeId, value, [...path, id], fieldModels))
                .onChange((path, change) => eventDispatcher.triggerChange(path, change));
            form.element.classList.remove('cloudy-ui-form');
            form.element.classList.add('cloudy-ui-embedded-form');
            form.appendTo(fieldset);

            data.form = form;

            const item = new SortableItem(fieldElement, id, { type });
            new SortableMenu(this, item).setHorizontal().appendTo(legend);
            return item;
        };

        const sortable = new Sortable();
        sortable.element.classList.add('cloudy-ui-sortable-field');

        for (let index = 0; index < value.length; index++) {
            sortable.add(await createItem(value[index].Type, value[index].Value, `original-${index}`), false);
        }

        const changesForContent = changeTracker.getFor(contentId, contentTypeId);
        const changesForField = changesForContent && changesForContent.changedFields.find(c => arrayEquals(path, c.path));

        if (changesForField && changesForField.type != 'array') {
            throw new Error(`Could not apply pending changes for content ${JSON.stringify(contentId)} of type ${contentTypeId} with path ${JSON.stringify(path)} as its pending change assume a type of '${changesForField.type}' but is now defined as 'array'`);
        }

        let newItemIndex = 0;

        if (changesForField && changesForField.changes) {
            for (const addition of changesForField.changes.filter(c => c.type == 'add')) {
                const index = +addition.id.substr('new-'.length);

                newItemIndex = Math.max(index + 1, newItemIndex);

                sortable.add(await createItem(addition.value.type, addition.value.value, `new-${index}`), false);
            }
        }

        sortable.onAdd(item => {
            eventDispatcher.triggerChange(path, { fieldType: 'array', type: 'add', value: { Type: item.data.type, Value: JSON.stringify({}) }, id: item.id }); // deep changes to value will be separate changes, denoted by id
        });

        const button = new Button('Add').onClick(() => menu.toggle());
        const menu = new PopupMenu(button.element);
        sortable.addFooter(menu);

        if (types.length) {
            types.forEach(item => menu.addItem(listItem => {
                listItem.setText(item.name);
                listItem.onClick(async () => sortable.add(await createItem(item.type, {}, `new-${newItemIndex++}`)));
            }));
        } else {
            menu.addItem(item => item.setDisabled().setText('(no items)'));
        }

        return sortable;
    }
}

export default new SortableBuilder();