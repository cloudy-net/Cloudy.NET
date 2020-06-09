import Blade from '../blade.js';
import FormBuilder from '../FormSupport/form-builder.js';
import Button from '../button.js';
import ContextMenu from '../ContextMenuSupport/context-menu.js';
import List from '../ListSupport/list.js';
import notificationManager from '../NotificationSupport/notification-manager.js';
import ListContentBlade from './list-content-blade.js';
import EditContentBlade from './edit-content-blade.js';
import HelpSectionLoader from './help-section-loader.js';
import ContentTypeProvider from '../DataSupport/content-type-provider.js';
import ContentTypeGroupProvider from '../DataSupport/content-type-group-provider.js';
import SingletonGetter from '../DataSupport/singleton-getter.js';
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
    constructor(app) {
        super();

        this.app = app;

        this.setTitle('What to edit');

        var update = async () => {
            const [contentTypes, contentTypeGroups] = await Promise.all([ContentTypeProvider.getAll(), ContentTypeGroupProvider.getAll()]);

            const items = [...contentTypes.map(t => ({ type: 'contentType', value: t })), ...contentTypeGroups.map(t => ({ type: 'contentTypeGroup', value: t }))];

            if (!items.length && !items.length) {
                var image = `<img class="cloudy-ui-help-illustration" src="./ContentAppSupport/images/undraw_coming_home_52ir.svg" alt="Illustration of an idyllic house with a direction sign, indicating a home.">`;
                var header = '<h2 class="cloudy-ui-help-heading">Welcome to your new home!</h2>';
                var text = '<p>It\'s time to create your first content type:</p>';
                var code = `<pre class="cloudy-ui-help-code">[ContentType("${guid}")]\n` +
                    'public class Page : IContent\n' +
                    '{\n' +
                    '    public string Id { get; set; }\n' +
                    '    public string ContentTypeId { get; set; }\n' +
                    '}</pre>';
                var textAfterCode = '<p>Save it, build it, and come back here!</p>';

                var helpContainer = document.createElement('cloudy-ui-help-container');
                helpContainer.innerHTML = image + header + text + code + textAfterCode;

                var reloadButton = new Button('Done').setPrimary().onClick(() => {
                    helpContainer.style.transition = '0.2s';
                    helpContainer.style.opacity = '0.3';

                    setTimeout(() => update(), 300);
                });
                var reloadButtonContainer = document.createElement('div');
                reloadButtonContainer.style.textAlign = 'center';
                reloadButtonContainer.append(reloadButton.element);

                helpContainer.append(reloadButtonContainer);
                this.setContent(helpContainer);

                return;
            }

            var list = new List();

            items.forEach(item => {
                var listItem = new ListItem();

                if (item.type == 'contentTypeGroup') {
                    var contentTypeGroup = item.value;
                    listItem.setText(contentTypeGroup.pluralName);
                } else {
                    var contentType = item.value;

                    if (contentType.contentTypeGroups.length) {
                        return;
                    }

                    if (!contentType.isSingleton) {
                        listItem.setText(contentType.pluralName);
                        listItem.onClick(() => {
                            listItem.setActive();
                            app.openAfter(new ListContentBlade(app, contentType).onClose(() => listItem.setActive(false)), this);
                        });
                    } else {
                        listItem.setText(contentType.name);
                        listItem.onClick(async () => {
                            listItem.setActive();
                            app.openAfter(new EditContentBlade(app, contentType, await SingletonGetter.get(contentType.id)).onClose(() => listItem.setActive(false)), this);
                        });
                    }

                    if (contentType.contentTypeActionModules.length) {
                        var menu = new ContextMenu();
                        listItem.setMenu(menu);
                        Promise.all(contentType.contentTypeActionModules.map(path => import(path)))
                            .then(actions => actions.forEach(module => module.default(menu, contentType, this, app)));
                    }

                    if (items.length == 1) {
                        listItem.element.click();
                    }
                }

                list.addItem(listItem);
            });

            this.setContent(list);
        };

        update();
    }
}

export default ListContentTypesBlade;