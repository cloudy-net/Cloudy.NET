import Blade from '../blade.js';
import Button from '../button.js';
import ContextMenu from '../ContextMenuSupport/context-menu.js';
import List from '../ListSupport/list.js';
import notificationManager from '../NotificationSupport/notification-manager.js';
import EditContentBlade from './edit-content-blade.js';
import RemoveContentBlade from './remove-content-blade.js';
import ListItem from '../ListSupport/list-item.js';



/* LIST CONTENT BLADE */

class ListContentBlade extends Blade {
    constructor(app, contentTypes) {
        super();

        this.app = app;
        this.contentTypes = contentTypes;

        this.contentTypesById = {};
        for (var contentType of contentTypes) {
            this.contentTypesById[contentType.id] = contentType;
        }
    }

    async open() {
        var creatableContentTypes = this.contentTypes.filter(t => !t.isSingleton);
        this.createNew = () => this.app.openAfter((this.contentTypes.length == 1 ? new EditContentBlade(this.app, this.contentTypes[0]) : new ChooseContentTypeBlade(this.app, creatableContentTypes)).onComplete(() => update()), this);
        this.setToolbar(new Button('New').setInherit().onClick(this.createNew));

        var actions = {};

        for (var contentType of this.contentTypes) {
            actions[contentType.id] = await Promise.all(contentType.listActionModules.map(path => import(path.indexOf('.') == 0 ? path : `../${path}`)));
        }

        var update = async () => {
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

            var list = new List();
            contentList.forEach(content => {
                var contentType = this.contentTypesById[content.contentTypeId];
                var listItem = new ListItem();
                var name;

                if (contentType.isNameable) {
                    name = contentType.nameablePropertyName ? content[contentType.nameablePropertyName] : content.name;

                    if (!name) {
                        name = `${contentType.name} ${content.id}`;
                    }
                } else {
                    name = content.id;
                }

                listItem.setText(name);

                if (contentType.isImageable) {
                    listItem.setImage(content.image || "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=");
                }

                if (this.contentTypes.length > 1) {
                    listItem.setSubText(contentType.name);
                }

                listItem.onClick(() => {
                    listItem.setActive();

                    var blade = new EditContentBlade(this.app, contentType, content)
                        .onComplete(() => {
                            var name;

                            if (contentType.isNameable) {
                                name = contentType.nameablePropertyName ? content[contentType.nameablePropertyName] : content.name;

                                if (!name) {
                                    name = `${contentType.name} ${content.id}`;
                                }
                            } else {
                                name = content.id;
                            }

                            listItem.setText(name);

                            if (contentType.isImageable) {
                                listItem.setImage(content.image || "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=");
                            }
                        })
                        .onClose(() => listItem.setActive(false));

                    this.app.openAfter(blade, this);
                });

                var menu = new ContextMenu();
                actions[contentType.id].forEach(module => module.default(menu, content, this, this.app));
                menu.addItem(item => {
                    item.setText('Remove');

                    if (contentType.isSingleton) {
                        item.setDisabled(true).onDisabledClick(() => notificationManager.addNotification(item => item.setText(`${name} can't be removed because it is a singleton - one (and only one) ${contentType.lowerCaseName} must always exist.`)));
                    } else {
                        item.onClick(() => this.app.openAfter(new RemoveContentBlade(this.app, contentType, content).onComplete(() => update()), this))
                    }
                });
                listItem.setMenu(menu);

                list.addItem(listItem);
            });

            this.setContent(list);
        };

        update();
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
                        this.app.close(this);
                    })
                    .onClose(() => listItem.setActive(false));

                this.app.openAfter(blade, this);
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