import Blade from '../blade.js';
import FormBuilder from '../FormSupport/form-builder.js';
import Button from '../button.js';
import ContextMenu from '../ContextMenuSupport/context-menu.js';
import List from '../ListSupport/list.js';
import notificationManager from '../NotificationSupport/notification-manager.js';
import EditContentBlade from './edit-content-blade.js';
import RemoveContentBlade from './remove-content-blade.js';
import HelpSectionLoader from './help-section-loader.js';



/* LIST CONTENT BLADE */

class ListContentBlade extends Blade {
    onEmptyCallbacks = [];

    constructor(app, contentType, contentTypeCount) {
        super();

        this.app = app;
        this.contentType = contentType;
    }

    async open() {
        this.setTitle(this.contentType.pluralName);

        var formBuilder = new FormBuilder(`Cloudy.CMS.Content[type=${this.contentType.id}]`, this.app);
        await formBuilder.fieldModels;

        this.createNew = () => this.app.openAfter(new EditContentBlade(this.app, this.contentType, formBuilder).onComplete(() => update()), this);
        this.setToolbar(new Button('New').setInherit().onClick(this.createNew));

        var actions = this.contentType.listActionModules.map(path => path[0] == '/' || path[0] == '.' ? import(path) : import(`${window.staticFilesBasePath}/${path}`));
        await Promise.all(actions);

        var update = async () => {
            var contentList;
            try {
                var response = await fetch(`Content/GetContentList?contentTypeId=${this.contentType.id}`, { credentials: 'include' });

                if (!response.ok) {
                    throw new Error(`${response.status} (${response.statusText})`);
                }

                contentList = await response.json();
            } catch (error) {
                notificationManager.addNotification(item => item.setText(`Could not get content list (${error.name}: ${error.message})`));
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
                    var blade = new EditContentBlade(this.app, this.contentType, formBuilder, content)
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

                            item.setText(name);
                        })
                        .onClose(() => item.setActive(false));

                    this.app.openAfter(blade, this);
                });

                var menu = new ContextMenu();
                item.setMenu(menu);
                Promise
                    .all(actions)
                    .then(actions => actions.forEach(module => module.default(menu, content, this, app)))
                    .then(() => {
                        menu.addItem(item => item.setText('Remove').onClick(() => app.openAfter(new RemoveContentBlade(app, contentType, formBuilder, content).onComplete(() => update()), this)));
                    });
                this.setContent(list);
            }));
        };

        update();
    }

    onEmpty(callback) {
        this.onEmptyCallbacks.push(callback);

        return this;
    }
}

export default ListContentBlade;