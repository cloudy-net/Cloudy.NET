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
    constructor(app, contentType) {
        super();

        this.app = app;
        this.contentType = contentType;
    }

    async open() {
        this.setTitle(this.contentType.pluralName);

        this.createNew = () => this.app.openAfter(new EditContentBlade(this.app, this.contentType).onComplete(() => update()), this);
        this.setToolbar(new Button('New').setInherit().onClick(this.createNew));

        var actions = await Promise.all(this.contentType.listActionModules.map(path => import(path.indexOf('.') == 0 ? path : `../${path}`)));

        var update = async () => {
            var contentList;
            try {
                var response = await fetch(`Content/GetContentList?contentTypeId=${this.contentType.id}`, { credentials: 'include' });

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
            }

            var list = new List();
            contentList.forEach(content => {
                var listItem = new ListItem();
                var name;

                if (this.contentType.isNameable) {
                    name = this.contentType.nameablePropertyName ? content[this.contentType.nameablePropertyName] : content.name;

                    if (!name) {
                        name = `${this.contentType.name} ${content.id}`;
                    }
                } else {
                    name = content.id;
                }

                listItem.setText(name);
                listItem.onClick(() => {
                    listItem.setActive();

                    var blade = new EditContentBlade(this.app, this.contentType, content)
                        .onComplete(() => {
                            var name;

                            if (this.contentType.isNameable) {
                                name = this.contentType.nameablePropertyName ? content[this.contentType.nameablePropertyName] : content.name;

                                if (!name) {
                                    name = `${this.contentType.name} ${content.id}`;
                                }
                            } else {
                                name = content.id;
                            }

                            listItem.setText(name);
                        })
                        .onClose(() => listItem.setActive(false));

                    this.app.openAfter(blade, this);
                });

                var menu = new ContextMenu();
                actions.forEach(module => module.default(menu, content, this, this.app));
                menu.addItem(item => item.setText('Remove').onClick(() => this.app.openAfter(new RemoveContentBlade(this.app, this.contentType, content).onComplete(() => update()), this)));
                listItem.setMenu(menu);

                list.addItem(listItem);
            });

            this.setContent(list);
        };

        update();
    }
}

export default ListContentBlade;