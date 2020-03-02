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
    constructor(app, contentType, contentTypeCount) {
        super();

        var createNew = () => app.openAfter(new EditContentBlade(app, contentType, formBuilder).onComplete(() => update()), this);

        this.setTitle(contentType.pluralName);
        this.setToolbar(new Button('New').setInherit().onClick(createNew));

        var actions = contentType.listActionModules.map(path => path[0] == '/' || path[0] == '.' ? import(path) : import(`${window.staticFilesBasePath}/${path}`));

        var formBuilder = new FormBuilder(`Cloudy.CMS.Content[type=${contentType.id}]`, app);
        var formFieldsPromise = formBuilder.fieldModels;

        var update = () => {
            var contentListPromise = fetch(`Content/GetContentList?contentTypeId=${contentType.id}`, { credentials: 'include' })
                .then(response => {
                    if (!response.ok) {
                        throw new Error(`${response.status} (${response.statusText})`);
                    }

                    return response.json();
                })
                .catch(error => notificationManager.addNotification(item => item.setText(`Could not get content list (${error.name}: ${error.message})`)));

            Promise.all([contentListPromise, formFieldsPromise, Promise.all(actions)]).then(([response, formFields]) => {
                if (response.length == 0) {
                    if (contentTypeCount == 2 && formFields.length == 0) {
                        HelpSectionLoader.load('content-list-no-properties', { contentTypeLowerCasePluralName: contentType.lowerCasePluralName, contentTypeLowerCaseName: contentType.lowerCaseName }, { }).then(element => this.setContent(element))

                        return;
                    } else {
                        HelpSectionLoader.load('content-list-empty', { contentTypeLowerCasePluralName: contentType.lowerCasePluralName, contentTypeLowerCaseName: contentType.lowerCaseName }, { createNew }).then(element => this.setContent(element))

                        return;
                    }
                }

                var list = new List();
                response.forEach(content => list.addItem(item => {
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
                    item.onClick(() => {
                        item.setActive();
                        var blade = new EditContentBlade(app, contentType, formBuilder, content)
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
                        app.openAfter(blade, this);
                    });

                    var menu = new ContextMenu();
                    item.setMenu(menu);
                    Promise
                        .all(actions)
                        .then(actions => actions.forEach(module => module.default(menu, content, this, app)))
                        .then(() => {
                            menu.addItem(item => item.setText('Remove').onClick(() => app.openAfter(new RemoveContentBlade(app, contentType, formBuilder, content).onComplete(() => update()), this)));
                        });
                }));
                this.setContent(list);
            });
        };

        update();
    }
}

export default ListContentBlade;