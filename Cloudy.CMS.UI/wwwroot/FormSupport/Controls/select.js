import FieldControl from '../field-control.js';
import ItemProvider from './select-item-provider.js';
import Blade from '../../blade.js';
import Button from '../../button.js';
import List from '../../ListSupport/list.js';
import ListItem from '../../ListSupport/list-item.js';
import SelectItemPreview from './select-item-preview.js';
import ContextMenu from '../../ContextMenuSupport/context-menu.js';



/* SELECT CONTROL */

class SelectControl extends FieldControl {
    open = null;

    constructor(fieldModel, value, app, blade) {
        var element = document.createElement('cloudy-ui-select');
        var empty = document.createElement('cloudy-ui-select-empty');
        var emptyText = document.createElement('cloudy-ui-select-empty-text');
        emptyText.innerText = '(none)';
        empty.append(emptyText);
        element.append(empty);
        var loading = document.createElement('cloudy-ui-select-loading');
        var loadingText = document.createElement('cloudy-ui-select-loading-text');
        loadingText.innerText = '(loading)';
        loading.append(loadingText);
        element.append(loading);
        var preview = new SelectItemPreview().appendTo(element);
        super(element);

        var update = item => {
            if (!item) {
                preview.element.style.display = 'none';
                loading.style.display = 'none';
                empty.style.display = '';

                return;
            }

            preview.element.style.display = '';
            empty.style.display = 'none';
            loading.style.display = 'none';

            preview.setImage(item.image);
            preview.setText(item.text);
            preview.setSubText(item.subText);
        };

        if (value) {
            preview.element.style.display = 'none';
            empty.style.display = 'none';
            loading.style.display = '';

            ItemProvider
                .get(fieldModel.descriptor.control.parameters['provider'], fieldModel.descriptor.control.parameters['type'], value)
                .then(item => {
                    if (item) {
                        update(item);
                    }
                });
        } else {
            update();
        }

        this.open = () => {
            var list = new ListItemsBlade(app, fieldModel)
                .onSelect(item => {
                    this.triggerChange(item.value);
                    update(item);
                    app.removeBlade(list);
                });

            app.addBladeAfter(list, blade);
        };

        new Button('Add').onClick(this.open).appendTo(empty);

        this.menu = new ContextMenu();

        this.menu.addItem(item => item.setText('Replace').onClick(this.open));

        if (!fieldModel.descriptor.isSortable) {
            this.menu.addItem(item => item.setText('Clear').onClick(() => { this.triggerChange(null); update(null); }));
        }

        preview.setMenu(this.menu);
        preview.onClick(() => this.menu.toggle());

        this.onSet(value => {
            update(value);
        });
    }
}



/* LIST ITEMS BLADE */

class ListItemsBlade extends Blade {
    onSelectCallbacks = [];

    constructor(app, fieldModel) {
        super();

        this.app = app;
        this.name = fieldModel.descriptor.label;
        this.provider = fieldModel.descriptor.control.parameters['provider'];
        this.type = fieldModel.descriptor.control.parameters['type'];
    }

    async open() {
        this.setTitle(`Select ${this.name.substr(0, 1).toLowerCase()}${this.name.substr(1)}`);

        //this.createNew = () => this.app.addBladeAfter(new EditContentBlade(this.app, this.contentType).onComplete(() => update()), this);
        this.setToolbar(new Button('New').setInherit()/*.onClick(this.createNew)*/);

        var list = new List();
        this.setContent(list);

        var update = async () => {
            var items = await ItemProvider.getAll(this.provider, this.type);

            if (!items.length) {
                var listItem = new ListItem();
                listItem.setDisabled();
                listItem.setText('(no items)');
                list.addItem(listItem);
                return;
            }

            items.forEach(item =>
                list.addItem(listItem => {
                    listItem.setImage(item.image);
                    listItem.setText(item.text);
                    listItem.setSubText(item.subText);
                    listItem.onClick(() => {
                        listItem.setActive();
                        this.onSelectCallbacks.forEach(callback => callback.apply(this, [item]));
                    });
                })
            );
        };

        update();
    }

    onSelect(callback) {
        this.onSelectCallbacks.push(callback);

        return this;
    }
}

export default SelectControl;