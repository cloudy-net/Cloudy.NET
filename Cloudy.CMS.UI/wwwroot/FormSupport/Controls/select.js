import FieldControl from '../field-control.js';
import ItemProvider from './select-item-provider.js';
import Blade from '../../blade.js';
import Button from '../../button.js';
import List from '../../components/list/list.js';
import ListItem from '../../components/list/list-item.js';
import SelectItemPreview from './select-item-preview.js';
import ContextMenu from '../../ContextMenuSupport/context-menu.js';
import notificationManager from '../../NotificationSupport/notification-manager.js';
import TabSystem from '../../TabSupport/tab-system.js';
import FormBuilder from '../form-builder.js';
import fieldDescriptorProvider from '../field-descriptor-provider.js';
import fieldModelBuilder from '../field-model-builder.js';
import urlFetcher from '../../util/url-fetcher.js';

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

        this.loadCreationAction();

        this.breadcrumbs = document.createElement('cloudy-ui-breadcrumbs');
        this.list = new List();
        this.setContent(this.breadcrumbs, this.list);

        this.listItems(this.parents);
    }

    async loadCreationAction() {
        var formId = await ItemProvider.getCreationForm(this.provider);

        if (!formId) {
            return;
        }

        this.setToolbar(new Button('New').setInherit().onClick(() => this.app.addBladeAfter(new CreateItemBlade(this.app, this.provider, formId, this.parents).onComplete(() => this.update(this.parents)), this)));
    }

    async listItems(parents) {
        this.list.element.style.opacity = 0.5;
        var query = {};

        if (parents.length) {
            query.parent = parents[parents.length - 1].value;
        }

        var items = await ItemProvider.getAll(this.provider, this.type, query);
        this.list.element.style.opacity = 1;
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

            if (this.item && this.item.value == item.value) {
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



/* CREATE ITEM */

class CreateItemBlade extends Blade {
    onCompleteCallbacks = [];

    constructor(app, provider, formId, parents) {
        super();

        this.app = app;
        this.provider = provider;
        this.formId = formId;
        this.item = {};

        this.element.addEventListener("keydown", (event) => {
            if ((String.fromCharCode(event.which).toLowerCase() == 's' && event.ctrlKey) || event.which == 19) { // 19 for Mac:s "Command+S"
                if (this.saveButton) {
                    this.saveButton.triggerClick();
                }
                event.preventDefault();
            }
        });
    }

    async open() {
        this.fieldModels = await fieldModelBuilder.getFieldModels(this.formId);
        this.formBuilder = new FormBuilder(this.app, this);

        this.setTitle('New item');

        this.buildForm();

        this.saveButton = new Button('Save')
            .setPrimary()
            .onClick(async () => {
                var result = await urlFetcher.fetch('SelectControl/CreateItem', {
                    credentials: 'include',
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        provider: this.provider,
                        item: JSON.stringify(this.item),
                    })
                }, 'Could not save item');

                if (!result.success) {
                    var errors = document.createElement('ul');
                    Object.entries(result.validationErrors).forEach(error => {
                        var item = document.createElement('li');
                        item.innerText = `${error[0]}: ${error[1]}`;
                        errors.append(item);
                    });
                    notificationManager.addNotification(item => item.setText(`Error saving item:`, errors));
                    return;
                }


                notificationManager.addNotification(item => item.setText(`Created item`));

                this.onCompleteCallbacks.forEach(callback => callback(this.content));
            });

        var cancelButton = new Button('Cancel').onClick(() => this.app.removeBlade(this));

        this.setFooter(this.saveButton, cancelButton);
    }

    async buildForm() {
        try {
            var groups = [...new Set((await fieldDescriptorProvider.getFor(this.formId)).map(fieldDescriptor => fieldDescriptor.group))].sort();

            if (groups.length == 1) {
                var form = this.formBuilder.build(this.content, this.fieldModels.filter(fieldModel => fieldModel.descriptor.group == groups[0]));

                this.setContent(form);
            } else {
                var tabSystem = new TabSystem();

                if (groups.indexOf(null) != -1) {
                    tabSystem.addTab('General', async () => {
                        var element = document.createElement('div');
                        var form = this.formBuilder.build(this.content, this.fieldModels.filter(fieldModel => fieldModel.descriptor.group == null));
                        form.appendTo(element);
                        return element;
                    });
                }

                groups.filter(g => g != null).forEach(group => tabSystem.addTab(group, async () => {
                    var element = document.createElement('div');
                    var form = this.formBuilder.build(this.content, this.fieldModels.filter(fieldModel => fieldModel.descriptor.group == group));
                    form.appendTo(element);
                    return element;
                }));

                this.setContent(tabSystem);
            }
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not build form --- ${error.message}`));
            throw error;
        }
    }

    onComplete(callback) {
        this.onCompleteCallbacks.push(callback);

        return this;
    }
}

export default SelectControl;