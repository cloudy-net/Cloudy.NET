import Blade from '../blade.js';
import Button from '../button.js';
import ContextMenu from '../ContextMenuSupport/context-menu.js';
import List from '../ListSupport/list.js';
import notificationManager from '../NotificationSupport/notification-manager.js';
import EditContentBlade from './edit-content-blade.js';
import RemoveContentBlade from './remove-content-blade.js';
import ListItem from '../ListSupport/list-item.js';
import Loading  from '../LoadingSupport/loading.js';
import urlFetcher from '../url-fetcher.js';

/* LIST CONTENT BLADE */

class ListContentBlade extends Blade {
    action = null;
    actions = null;

    constructor(app, contentTypes, taxonomy) {
        super();

        this.app = app;
        this.contentTypes = contentTypes;
        this.taxonomy = taxonomy;

        this.contentTypesById = {};
        for (var contentType of contentTypes) {
            this.contentTypesById[contentType.id] = contentType;
        }

        this.setTitle(`${taxonomy.pluralName}`);

        this._loading = new Loading(this.element);
    }

    async open() {
        this.contentTypeActions = {};

        for (var contentType of this.contentTypes) {
            this.contentTypeActions[contentType.id] = await Promise.all(contentType.listActionModules.map(path => import(path.indexOf('.') == 0 ? path : `../${path}`)));
        }

        this.actions = {};

        this.breadcrumbs = document.createElement('cloudy-ui-breadcrumbs');
        this.list = new List();
        this.setContent(this.breadcrumbs, this.list);

        await this.listItems([]);

        if (this.actions[this.action]) {
            this.actions[this.action]();
        }
    }

    async listItems(parents) {
        var creatableContentTypes = this.contentTypes.filter(t => !t.isSingleton);
        this.createNew = () => this.app.addBladeAfter((this.contentTypes.length == 1 ? new EditContentBlade(this.app, this.contentTypes[0], { parentId: parents.length ? parents[parents.length - 1].id : null }) : new ChooseContentTypeBlade(this.app, creatableContentTypes)).onComplete(() => this.listItems(parents)), this);
        this.setToolbar(new Button('New').setInherit().onClick(this.createNew));

        this.list.element.style.opacity = 0.5;

        var url = 'ContentList/Get?';

        url += this.contentTypes.map((t, i) => `contentTypeIds[${i}]=${t.id}`).join('&');

        if (parents.length) {
            url += `&parent=${parents[parents.length - 1].id}`;
        }
        this._loading.turnOn(3000);
        var contentList = await urlFetcher.fetch(url, { credentials: 'include' }, 'Could not load content list').finally(_ => this._loading.turnOf());
        this.updateBreadcrumbs(parents);
        this.list.element.style.opacity = 1;
        this.list.clear();

        if (!contentList.items.length) {
            var listItem = new ListItem();
            listItem.setText(`(no ${this.taxonomy.lowerCasePluralName})`);
            listItem.setDisabled();
            this.list.addItem(listItem);

            return;
        }

        var updateListItem = (listItem, item, contentType) => {
            listItem.setText(item.name);

            if (contentType.isImageable) {
                listItem.setImage(item.image || "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=");
            }

            if (this.contentTypes.length > 1) {
                listItem.setSubText(contentType.name);
            }
        };

        contentList.items.forEach(item => {
            var contentType = this.contentTypes[0];
            var listItem = new ListItem();
            this.list.addItem(listItem);

            updateListItem(listItem, item, contentType);

            if (contentList.itemChildrenCounts[JSON.stringify(item.keys)]) {
                var folder = document.createElement('cloudy-ui-list-item-folder');
                folder.addEventListener('click', event => this.listItems([...parents, item]));
                listItem.element.append(folder);
            }

            listItem.onClick(async () => {
                listItem.setActive();
                var blade = new EditContentBlade(this.app, contentType, item.keys)
                    .onComplete(() => updateListItem(listItem, item, contentType))
                    .onClose(() => {
                        listItem.setActive(false);
                    });
                await this.app.addBladeAfter(blade, this);
            });
          

            var menu = new ContextMenu();
            this.contentTypeActions[contentType.id].forEach(module => module.default(menu, item.keys, this, this.app));
            menu.addItem(item => {
                item.setText('Remove');

                if (contentType.isSingleton) {
                    item.setDisabled(true).onDisabledClick(() => notificationManager.addNotification(item => item.setText(`${name} can't be removed because it is a singleton - one (and only one) ${contentType.lowerCaseName} must always exist.`)));
                } else {
                    item.onClick(() => this.app.addBladeAfter(new RemoveContentBlade(this.app, contentType, item.keys).onComplete(() => this.listItems(parents)), this))
                }
            });
            listItem.setMenu(menu);
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

        parents.forEach((content, i) => {
            var contentType = this.contentTypes[0];

            if (contentType.isNameable) {
                var name = contentType.nameablePropertyName ? content[contentType.nameablePropertyName] : content.name;

                if (!name) {
                    name = `${contentType.name} ${content.id}`;
                }
            } else {
                var name = content.id;
            }

            var breadcrumb = document.createElement('cloudy-ui-breadcrumb-separator');
            this.breadcrumbs.append(breadcrumb);

            var breadcrumb = document.createElement('cloudy-ui-breadcrumb');
            breadcrumb.innerText = name;
            this.breadcrumbs.append(breadcrumb);

            if (i == parents.length - 1) {
                breadcrumb.classList.add('cloudy-ui-active');
                return;
            }

            breadcrumb.classList.add('cloudy-ui-clickable');
            breadcrumb.addEventListener('click', () => this.listItems(parents.slice(0, i)));
        });
    }
}



/* CHOOSE CONTENT TYPE BLADE */

class ChooseContentTypeBlade extends Blade {
    onCompleteCallbacks = [];

    constructor(app, contentTypes) {
        super();

        this.app = app;
        this.contentTypes = contentTypes;
        this.setTitle('Choose content type');
    }

    async open() {
        var list = new List();
        this.contentTypes.forEach(contentType => {
            var listItem = new ListItem();

            listItem.setText(contentType.name);
            listItem.onClick(() => {
                listItem.setActive();

                var blade = new EditContentBlade(this.app, contentType)
                    .onComplete(() => {
                        this.onCompleteCallbacks.forEach(callback => callback());
                        this.app.removeBlade(this);
                    })
                    .onClose(() => listItem.setActive(false));

                this.app.addBladeAfter(blade, this);
            });

            list.addItem(listItem);
        });
        this.setContent(list);
    }

    onComplete(callback) {
        this.onCompleteCallbacks.push(callback);

        return this;
    }
}



export default ListContentBlade;