import FieldControl from '../field-control.js';
import ItemProvider from './select-item-provider.js';
import Blade from '../../blade.js';
import Button from '../../button.js';
import List from '../../ListSupport/list.js';
import ListItem from '../../ListSupport/list-item.js';
import SelectItemPreview from './select-item-preview.js';
import ContextMenu from '../../ContextMenuSupport/context-menu.js';
import notificationManager from '../../NotificationSupport/notification-manager.js';



/* SELECT CONTROL */

class SelectControl extends FieldControl {
    item = null;
    parents = [];

    constructor(fieldModel, value, app, blade) {
        super(document.createElement('cloudy-ui-select'));

        this.fieldModel = fieldModel;
        this.app = app;
        this.blade = blade;

        this.empty = document.createElement('cloudy-ui-select-empty');
        var emptyText = document.createElement('cloudy-ui-select-empty-text');
        emptyText.innerText = '(none)';
        this.empty.append(emptyText);
        this.element.append(this.empty);

        this.loading = document.createElement('cloudy-ui-select-loading');
        var loadingText = document.createElement('cloudy-ui-select-loading-text');
        loadingText.innerText = '(loading)';
        this.loading.append(loadingText);
        this.element.append(this.loading);
        this.preview = new SelectItemPreview().appendTo(this.element);

        if (value) {
            this.preview.element.style.display = 'none';
            this.empty.style.display = 'none';
            this.loading.style.display = '';

            ItemProvider
                .get(fieldModel.descriptor.control.parameters['provider'], fieldModel.descriptor.control.parameters['type'], value)
                .then(result => {
                    if (result) {
                        this.item = result.item;
                        this.parents = result.parents;
                    }

                    this.update();

                    if(!result) {
                        notificationManager.addNotification(item => item.setText(`Could not get item \`${value}\` of type \`${fieldModel.descriptor.control.parameters['type']}\` for select control \`${fieldModel.descriptor.control.parameters['provider']}\``));
                    }
                });
        } else {
            this.update();
        }

        new Button('Add').onClick(() => this.open()).appendTo(this.empty);

        this.menu = new ContextMenu();

        this.menu.addItem(item => item.setText('Replace').onClick(() => this.open()));

        if (!fieldModel.descriptor.isSortable) {
            this.menu.addItem(item => item.setText('Clear').onClick(() => {
                this.item = null;
                this.parents = [];
                this.triggerChange(null);
                this.update();
            }));
        }

        this.preview.setMenu(this.menu);
        this.preview.onClick(() => setTimeout(() => this.menu.toggle(), 1));

        this.onSet(value => {
            update(value);
        });
    }

    update() {
        if (!this.item) {
            this.preview.element.style.display = 'none';
            this.loading.style.display = 'none';
            this.empty.style.display = '';

            return;
        }

        this.preview.element.style.display = '';
        this.empty.style.display = 'none';
        this.loading.style.display = 'none';

        this.preview.setImage(this.item.image);
        this.preview.setText(this.item.text);
        this.preview.setSubText(this.item.subText);
    }

    open() {
        var list = new ListItemsBlade(this.app, this.fieldModel, this.item, this.parents)
            .onComplete(result => {
                this.item = result.item;
                this.parents = result.parents;
                this.triggerChange(result.item.value);
                this.update();
                this.app.removeBlade(list);
            });

        this.app.addBladeAfter(list, this.blade);
    }
}



/* LIST ITEMS BLADE */

class ListItemsBlade extends Blade {
    onCompleteCallbacks = [];

    constructor(app, fieldModel, item, parents) {
        super();

        this.app = app;
        this.name = fieldModel.descriptor.label;
        this.provider = fieldModel.descriptor.control.parameters['provider'];
        this.type = fieldModel.descriptor.control.parameters['type'];

        this.item = item;
        this.parents = parents;
    }

    async open() {
        this.setTitle(`Select ${this.name.substr(0, 1).toLowerCase()}${this.name.substr(1)}`);

        //this.createNew = () => this.app.addBladeAfter(new EditContentBlade(this.app, this.contentType).onComplete(() => update()), this);
        this.setToolbar(new Button('New').setInherit()/*.onClick(this.createNew)*/);

        this.breadcrumbs = document.createElement('cloudy-ui-breadcrumbs');
        this.list = new List();
        this.setContent(this.breadcrumbs, this.list);

        this.listItems(this.parents);
    }

    async listItems(parents) {
        this.list.element.opacity = 0.5;
        var query = {};

        if (parents.length) {
            query.parent = parents[parents.length - 1].value;
        }

        var items = await ItemProvider.getAll(this.provider, this.type, query);
        this.list.element.opacity = 1;
        this.list.clear();

        this.updateBreadcrumbs(parents);

        if (!items.length) {
            var listItem = new ListItem();
            listItem.setDisabled();
            listItem.setText('(no items)');
            this.list.addItem(listItem);
            return;
        }

        items.forEach(item => {
            var listItem = new ListItem();
            this.list.addItem(listItem);

            if (item.value == this.item.value) {
                listItem.setActive();
            }
            listItem.setImage(item.image);
            listItem.setText(item.text);
            listItem.setSubText(item.subText);

            if (item.isSelectable) {
                listItem.onClick(() => {
                    listItem.setActive();
                    this.onCompleteCallbacks.forEach(callback => callback.apply(this, [{ item: item, parents: this.parents }]));
                });
            }

            if (item.isParent) {
                var folder = document.createElement('cloudy-ui-list-item-folder');
                folder.addEventListener('click', () => this.listItems([...parents, item]));
                listItem.element.append(folder);
            }

            if (item.isSelectable) {
                var menu = new ContextMenu();
                menu.addItem(menuItem => {
                    menuItem.setText('Copy');
                    menuItem.onClick(() => navigator.clipboard.writeText(item.value));
                });
                listItem.setMenu(menu);
            }
        });
    }

    updateBreadcrumbs(parents) {
        [...this.breadcrumbs.childNodes].forEach(element => element.remove());

        if (!parents.length) {
            this.breadcrumbs.style.display = 'none';
            return;
        }

        this.breadcrumbs.style.display = '';

        var breadcrumb = document.createElement('cloudy-ui-breadcrumb');
        breadcrumb.innerText = 'Top';
        breadcrumb.classList.add('cloudy-ui-clickable');
        breadcrumb.addEventListener('click', () => this.listItems([]));
        this.breadcrumbs.append(breadcrumb);

        parents.forEach((item, i) => {
            var breadcrumb = document.createElement('cloudy-ui-breadcrumb-separator');
            this.breadcrumbs.append(breadcrumb);

            var breadcrumb = document.createElement('cloudy-ui-breadcrumb');
            breadcrumb.innerText = item.text;
            this.breadcrumbs.append(breadcrumb);

            if (i == parents.length - 1) {
                breadcrumb.classList.add('cloudy-ui-active');
                return;
            }

            breadcrumb.classList.add('cloudy-ui-clickable');
            breadcrumb.addEventListener('click', () => this.listItems(parents.slice(0, i)));
        });
    }

    onComplete(callback) {
        this.onCompleteCallbacks.push(callback);

        return this;
    }
}

export default SelectControl;