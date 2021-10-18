import Blade from '../blade.js';
import ContextMenu from '../ContextMenuSupport/context-menu.js';
import List from '../ListSupport/list.js';
import ListContentBlade from './list-content-blade.js';
import EditContentBlade from './edit-content-blade.js';
import ContentTypeProvider from './utils/content-type-provider.js';
import ContentTypeGroupProvider from './utils/content-type-group-provider.js';
import SingletonGetter from './utils/singleton-getter.js';
import ListItem from '../ListSupport/list-item.js';


/* LIST CONTENT TYPES BLADE */

var guid = uuidv4();

function uuidv4() { // https://stackoverflow.com/a/2117523
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

class ListContentTypesBlade extends Blade {
    action = null;
    actions = null;

    constructor(app) {
        super();
        this.app = app;
        this._close.remove();
    }

    async open() {
        this.setTitle('What to edit');

        const list = new List();
        this.setContent(list);

        const [contentTypes, contentTypeGroups] = await Promise.all([
            ContentTypeProvider.getAll(),
            ContentTypeGroupProvider.getAll(),
        ]);

        const items = [
            ...contentTypes.map(t => ({ id: t.id, type: 'contentType', value: t })),
            ...contentTypeGroups.map(t => ({ id: t.id, type: 'contentTypeGroup', value: t }))
        ];

        this.actions = {};

        items.forEach(item => {
            const listItem = new ListItem();
            list.addItem(listItem);
            
            if (item.type == 'contentTypeGroup') {
                var contentTypeGroup = item.value;
                var groupContentTypes = contentTypes.filter(t => contentTypeGroup.contentTypes.includes(t.id));
                listItem.setText(contentTypeGroup.pluralName);
                listItem.onClick(async () => {
                    listItem.setActive();
                    var blade = new ListContentBlade(this.app, groupContentTypes, contentTypeGroup)
                        .onClose(() => {
                            listItem.setActive(false);
                        });
                    await this.app.addBladeAfter(blade, this);
                });

                return;
            }

            var contentType = item.value;

            if (contentType.contentTypeGroups.length) {
                return;
            }

            if (contentType.isSingleton) {
                listItem.setText(contentType.name);

                listItem.onClick(async () => {
                    listItem.setActive();
                    var content = await SingletonGetter.get(contentType.id);
                    var blade = new EditContentBlade(this.app, contentType, content)
                        .onClose(() => {
                            listItem.setActive(false);
                        });
                    await this.app.addBladeAfter(blade, this);
                });
            } else {
                listItem.setText(contentType.pluralName);

                listItem.onClick(async () => {
                    listItem.setActive();
                    var blade = new ListContentBlade(this.app, [contentType], contentType)
                        .onClose(() => {
                            listItem.setActive(false);
                        });
                    await this.app.addBladeAfter(blade, this);
                });
            }

            if (contentType.contentTypeActionModules.length) {
                var menu = new ContextMenu();
                listItem.setMenu(menu);
                Promise.all(contentType.contentTypeActionModules.map(path => import(path)))
                    .then(actions => actions.forEach(module => module.default(menu, contentType, this, this.app)));
            }
        });

        if (this.actions[this.action]) {
            this.actions[this.action]();
        }
    }
}

export default ListContentTypesBlade;