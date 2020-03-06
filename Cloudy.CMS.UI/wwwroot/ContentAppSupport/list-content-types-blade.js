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

        var update = () => ContentTypeProvider.getAll()
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
                                    var blade = new ListContentBlade(app, contentType, contentTypes.length).onClose(() => item.setActive(false));
                                    blade.onEmpty(async function() {
                                        if (contentTypes.length == 2 /*&& this.formBuilder.fieldModels.length == 0*/) {
                                            //var section = await HelpSectionLoader.load('content-list-no-properties', { contentTypeLowerCasePluralName: contentType.lowerCasePluralName, contentTypeLowerCaseName: contentType.lowerCaseName }, {});

                                            //this.setContent(section);

                                            //return;
                                        } else {
                                            var section = await HelpSectionLoader.load('content-list-empty', { contentTypeLowerCasePluralName: contentType.lowerCasePluralName, contentTypeLowerCaseName: contentType.lowerCaseName }, { createNew: this.createNew });

                                            this.setContent(section);

                                            return;
                                        }
                                    });
                                    blade.onSelect(function (content) {
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

                                                item.setText(name);
                                            })
                                            .onClose(() => item.setActive(false));

                                        this.app.openAfter(blade, this);
                                    });
                                    app.openAfter(blade, this);
                                });
                            } else {
                                var content = fetch(`Content/GetSingleton?id=${contentType.id}`, { credentials: 'include' })
                                    .then(response => {
                                        if (!response.ok) {
                                            throw new Error(`${response.status} (${response.statusText})`);
                                        }

                                        return response.json();
                                    })
                                    .catch(error => notificationManager.addNotification(item => item.setText(`Could not get singleton (${error.name}: ${error.message})`)));

                                item.onClick(() => {
                                    content.then(content => {
                                        item.setActive();
                                        app.openAfter(
                                            new EditContentBlade(app, contentType, content)
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