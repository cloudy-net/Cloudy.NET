import Blade from '../blade.js';
import Button from '../button.js';
import ContextMenu from '../ContextMenuSupport/context-menu.js';
import List from '../ListSupport/list.js';
import notificationManager from '../NotificationSupport/notification-manager.js';
import EditContentBlade from './edit-content-blade.js';
import RemoveContentBlade from './remove-content-blade.js';
import ListItem from '../ListSupport/list-item.js';
import state from '../state.js';



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
    }

    async open() {
        var creatableContentTypes = this.contentTypes.filter(t => !t.isSingleton);
        this.createNew = () => this.app.addBladeAfter((this.contentTypes.length == 1 ? new EditContentBlade(this.app, this.contentTypes[0]) : new ChooseContentTypeBlade(this.app, creatableContentTypes)).onComplete(() => update()), this);
        this.setToolbar(new Button('New').setInherit().onClick(this.createNew));

        var contentTypeActions = {};

        for (var contentType of this.contentTypes) {
            contentTypeActions[contentType.id] = await Promise.all(contentType.listActionModules.map(path => import(path.indexOf('.') == 0 ? path : `../${path}`)));
        }

        var contentList;
        try {
            var response = await fetch(`Content/GetContentList?${this.contentTypes.map((t, i) => `contentTypeId[${i}]=${t.id}`).join('&')}`, { credentials: 'include' });

            if (!response.ok) {
                var text = await response.text();

                if (text) {
                    throw new Error(text.split('\n')[0]);
                } else {
                    text = response.statusText;
                }

                throw new Error(`${response.status} (${text})`);
            }

            contentList = await response.json();
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not get content list (${error.message})`));
            throw error;
        }

        this.actions = {};

        var list = new List();
        this.setContent(list);

        if (!contentList.length) {
            var listItem = new ListItem();
            listItem.setText(`(no ${this.taxonomy.lowerCasePluralName})`);
            listItem.setDisabled();
            list.addItem(listItem);

            return;
        }

        var updateListItem = (listItem, content, contentType) => {
            if (contentType.isNameable) {
                var name = contentType.nameablePropertyName ? content[contentType.nameablePropertyName] : content.name;

                if (!name) {
                    name = `${contentType.name} ${content.id}`;
                }
            } else {
                var name = content.id;
            }

            listItem.setText(name);

            if (contentType.isImageable) {
                listItem.setImage(content.image || "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=");
            }

            if (this.contentTypes.length > 1) {
                listItem.setSubText(contentType.name);
            }
        };

        contentList.forEach(content => {
            var contentType = this.contentTypesById[content.contentTypeId];
            var listItem = new ListItem();
            list.addItem(listItem);

            updateListItem(listItem, content, contentType);

            listItem.onClick(() => state.set(content.id, this));

            this.actions[content.id] = async () => {
                listItem.setActive();

                var blade = new EditContentBlade(this.app, contentType, content)
                    .onComplete(() => updateListItem(listItem, content, contentType))
                    .onClose(() => listItem.setActive(false));

                await this.app.addBladeAfter(blade, this);
            };

            var menu = new ContextMenu();
            contentTypeActions[contentType.id].forEach(module => module.default(menu, content, this, this.app));
            menu.addItem(item => {
                item.setText('Remove');

                if (contentType.isSingleton) {
                    item.setDisabled(true).onDisabledClick(() => notificationManager.addNotification(item => item.setText(`${name} can't be removed because it is a singleton - one (and only one) ${contentType.lowerCaseName} must always exist.`)));
                } else {
                    item.onClick(() => this.app.addBladeAfter(new RemoveContentBlade(this.app, contentType, content).onComplete(() => update()), this))
                }
            });
            listItem.setMenu(menu);
        });

        if (this.actions[this.action]) {
            this.actions[this.action]();
        }
    }

    async stateUpdate() {
        var action = state.getFor(this);

        if (this.action == action) {
            return;
        }

        this.action = action;

        await this.app.removeBladeAfter(this);

        if (!this.action) {
            return;
        }

        if (!this.actions) {
            return;
        }

        if (!this.actions[this.action]) {
            notificationManager.addNotification(item => item.setText(`Unknown action: \`${this.action}\``));
            return;
        }

        await this.actions[this.action]();
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