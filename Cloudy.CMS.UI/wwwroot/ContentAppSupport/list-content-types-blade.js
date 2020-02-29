import Blade from '../blade.js';
import FormBuilder from '../FormSupport/form-builder.js';
import Button from '../button.js';
import ContextMenu from '../ContextMenuSupport/context-menu.js';
import List from '../ListSupport/list.js';
import notificationManager from '../NotificationSupport/notification-manager.js';
import ListContentBlade from './list-content-blade.js';
import EditContentBlade from './edit-content-blade.js';



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

        this.setTitle('What to edit');

        var update = () =>
            fetch('Content/GetContentTypeList', { credentials: 'include' })
                .catch(error => notificationManager.addNotification(item => item.setText(`Could not get content types (${error.name}: ${error.message})`)))
                .then(response => response.json())
                .then(contentTypes => {
                    if (!contentTypes.length) {
                        var image = `<img class="cloudy-ui-help-illustration" src="${window.staticFilesBasePath}/ContentAppSupport/images/undraw_coming_home_52ir.svg" alt="Illustration of an idyllic house with a direction sign, indicating a home.">`;
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

                    if (contentTypes.length) {
                        //list.addSubHeader('General');
                        contentTypes.forEach(contentType => list.addItem(item => {
                            item.setText(contentType.pluralName);

                            if (!contentType.isSingleton) {
                                item.onClick(() => {
                                    item.setActive();
                                    app.openAfter(new ListContentBlade(app, contentType, contentTypes.length).onClose(() => item.setActive(false)), this);
                                });
                            } else {
                                var formBuilder = new FormBuilder(`Cloudy.CMS.Content[type=${contentType.id}]`, app);
                                var content = fetch(`Content/GetSingleton?id=${contentType.id}`, { credentials: 'include' })
                                    .catch(error => notificationManager.addNotification(item => item.setText(`Could not get singleton (${error.name}: ${error.message})`)))
                                    .then(response => response.json());

                                item.onClick(() => {
                                    Promise.all([formBuilder.fieldModels, content]).then(results => {
                                        item.setActive();
                                        app.openAfter(
                                            new EditContentBlade(app, contentType, formBuilder, results[1])
                                                .onClose(() => item.setActive(false)),
                                            this);
                                    });
                                });
                            }

                            var actions = contentType.contentTypeActionModules.map(path => path[0] == '/' || path[0] == '.' ? import(path) : import(`${window.staticFilesBasePath}/${path}`));

                            if (actions.length) {
                                var menu = new ContextMenu();
                                item.setMenu(menu);
                                Promise.all(actions).then(actions => actions.forEach(module => module.default(menu, contentType, this, app)));
                            }

                            if (contentTypes.length == 1) {
                                item.element.click();
                            }
                        }));
                    }

                    this.setContent(list);
                });

        update();
    }
}

export default ListContentTypesBlade;