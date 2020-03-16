import Blade from '../blade.js';
import Button from '../button.js';
import ContextMenu from '../ContextMenuSupport/context-menu.js';
import List from '../ListSupport/list.js';
import notificationManager from '../NotificationSupport/notification-manager.js';
import EditContentBlade from './edit-content-blade.js';
import RemoveContentBlade from './remove-content-blade.js';



/* LIST CONTENT BLADE */

class ListContentBlade extends Blade {
    onEmptyCallbacks = [];
    onSelectCallbacks = [];

    constructor(app, contentType, contentTypeCount) {
        super();

        this.app = app;
        this.contentType = contentType;
    }

    async open() {
        this.setTitle(this.contentType.pluralName);

        this.createNew = () => this.app.openAfter(new EditContentBlade(this.app, this.contentType).onComplete(() => update()), this);
        this.setToolbar(new Button('New').setInherit().onClick(this.createNew));

        var actions = this.contentType.listActionModules.map(path => path[0] == '/' || path[0] == '.' ? import(path) : import(`${window.staticFilesBasePath}/${path}`));
        await Promise.all(actions);

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

            if (contentList.length == 0) {
                this.onEmptyCallbacks.forEach(callback => callback.apply(this));
                return;
            }

            var list = new List();
            contentList.forEach(content => list.addItem(item => {
                var name;

                if (this.contentType.isNameable) {
                    name = this.contentType.nameablePropertyName ? content[this.contentType.nameablePropertyName] : content.name;

                    if (!name) {
                        name = `${this.contentType.name} ${content.id}`;
                    }
                } else {
                    name = content.id;
                }

                item.setText(name);
                item.onClick(() => {
                    item.setActive();
                    this.onSelectCallbacks.forEach(callback => callback.apply(this, [content]));
                });

                var menu = new ContextMenu();
                item.setMenu(menu);

                (async () => {
                    var modules = await Promise.all(actions);
                    modules.forEach(module => module.default(menu, content, this, this.app));
                    menu.addItem(item => item.setText('Remove').onClick(() => this.app.openAfter(new RemoveContentBlade(this.app, this.contentType, content).onComplete(() => update()), this)));
                })();

                this.setContent(list);
            }));
        };

        update();
    }

    onEmpty(callback) {
        this.onEmptyCallbacks.push(callback);

        return this;
    }

    onSelect(callback) {
        this.onSelectCallbacks.push(callback);

        return this;
    }
}

export default ListContentBlade;