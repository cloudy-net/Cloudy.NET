import App from '../app.js';
import Blade from '../blade.js';
import FormBuilder from '../FormSupport/form-builder.js';
import Button from '../button.js';
import LinkButton from '../link-button.js';
import ContextMenu from '../ContextMenuSupport/context-menu.js';
import List from '../ListSupport/list.js';
import TabSystem from '../TabSupport/tab-system.js';



/* APP */

class ContentApp extends App {
    constructor() {
        super();
        this.open(new ListContentTypesBlade(this));
    }
};

export default ContentApp;



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
            fetch('ContentApp/GetContentTypes', { credentials: 'include' })
                .then(response => response.json())
                .catch(e => console.log(e))
                .then(contentTypes => {
                    if (!contentTypes.length) {
                        var image = `<img class="poetry-ui-help-illustration" src="${window.staticFilesBasePath}/ContentAppSupport/images/undraw_coming_home_52ir.svg" alt="Illustration of an idyllic house with a direction sign, indicating a home.">`;
                        var header = '<h2 class="poetry-ui-help-heading">Welcome to your new home!</h2>';
                        var text = '<p>It\'s time to create your first content type:</p>';
                        var code = `<pre class="poetry-ui-help-code">[ContentType("${guid}")]\n` +
                            'public class Page : IContent\n' +
                            '{\n' +
                            '    public string Id { get; set; }\n' +
                            '    public string ContentTypeId { get; set; }\n' +
                            '}</pre>';
                        var textAfterCode = '<p>Save it, build it, and come back here!</p>';

                        var helpContainer = document.createElement('poetry-ui-help-container');
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
                                var content = fetch(`ContentApp/GetSingleton?id=${contentType.id}`, { credentials: 'include' }).then(response => response.json());

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



/* LIST CONTENT BLADE */

class ListContentBlade extends Blade {
    constructor(app, contentType, contentTypeCount) {
        super();

        var createNew = () => app.openAfter(new EditContentBlade(app, contentType, formBuilder).onSave(() => update()), this);

        this.setTitle(contentType.pluralName);
        this.setToolbar(new Button('New').setInherit().onClick(createNew));

        var actions = contentType.listActionModules.map(path => path[0] == '/' || path[0] == '.' ? import(path) : import(`${window.staticFilesBasePath}/${path}`));

        var formBuilder = new FormBuilder(`Cloudy.CMS.Content[type=${contentType.id}]`, app);
        var formFieldsPromise = formBuilder.fieldModels;

        var update = () => {
            var contentListPromise = fetch(`ContentApp/GetContentList?contentTypeId=${contentType.id}`, { credentials: 'include' }).then(response => response.json());

            Promise.all([contentListPromise, formFieldsPromise, Promise.all(actions)]).then(([response, formFields]) => {
                if (response.length == 0) {
                    if (contentTypeCount == 1 && formFields.length == 0) {
                        var image = `<img class="poetry-ui-help-illustration" src="${window.staticFilesBasePath}/ContentAppSupport/images/undraw_suburbs_8b83.svg" alt="Illustration of a row of houses.">`;
                        var header1 = `<h2 class="poetry-ui-help-heading">No ${contentType.pluralName[0].toLowerCase()}${contentType.pluralName.substr(1)}, no properties … yet</h2>`;
                        var text1 = '<p>Your content type looks a bit empty. Let\'s add some properties!</p>';
                        var text2 = '<p>Try implementing INameable:</p>';
                        var code = '<pre class="poetry-ui-help-code">public class MyClass : …, INameable\n' +
                            '{\n' +
                            '    …\n' +
                            '    public string Name { get; set; }\n' +
                            '}</pre>';

                        var helpContainer = document.createElement('poetry-ui-help-container');
                        helpContainer.innerHTML = image + header1 + text1 + text2 + code;
                        this.setContent(helpContainer);

                        return;
                    } else {
                        var image = `<img class="poetry-ui-help-illustration" src="${window.staticFilesBasePath}/ContentAppSupport/images/undraw_remotely_2j6y.svg" alt="Illustration of a row of houses.">`;
                        var header1 = `<h2 class="poetry-ui-help-heading">There's nothing here</h2>`;
                        var text1 = `<p>You haven’t created any ${contentType.pluralName[0].toLowerCase()}${contentType.pluralName.substr(1)} yet. Let’s do it!</p>`;

                        var button = new Button(`Get to work`).setPrimary().onClick(createNew);
                        var buttonContainer = document.createElement('div');
                        buttonContainer.style.textAlign = 'center';
                        buttonContainer.append(button.element);

                        var helpContainer = document.createElement('poetry-ui-help-container');
                        helpContainer.innerHTML = image + header1 + text1;

                        helpContainer.append(buttonContainer);
                        this.setContent(helpContainer);

                        return;
                    }
                }

                var list = new List();
                response.forEach(content => list.addItem(item => {
                    item.setText(contentType.isNameable ? (contentType.nameablePropertyName ? content[contentType.nameablePropertyName] : content.name) : content.id);
                    item.onClick(() => {
                        item.setActive();
                        app.openAfter(new EditContentBlade(app, contentType, formBuilder, content).onSave(() => item.setText(contentType.isNameable ? content.name : content.id)).onClose(() => item.setActive(false)), this);
                    });

                    if (actions.length) {
                        var menu = new ContextMenu();
                        item.setMenu(menu);
                        Promise.all(actions).then(actions => actions.forEach(module => module.default(menu, content, this, app)));
                    }
                }));
                this.setContent(list);
            });
        };

        update();
    }
}



/* EDIT CONTENT */

class EditContentBlade extends Blade {
    onSaveCallbacks = [];

    constructor(app, contentType, formBuilder, content) {
        super();

        if (!content) {
            content = {};
        }

        if (content.id) {
            if (contentType.isNameable && content.name) {
                this.setTitle(`Edit ${content.name}`);
            } else {
                this.setTitle(`Edit ${contentType.name}`);
            }

            if (contentType.isRoutable) {
                fetch(`ContentApp/GetUrl?id=${encodeURIComponent(content.id)}&contentTypeId=${encodeURIComponent(content.contentTypeId)}`, {
                    credentials: 'include',
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(response => response.text())
                    .then(url => {
                        if (!url) {
                            return;
                        }

                        url = url.substr(1, url.length - 2);

                        this.setToolbar(new LinkButton('View', `${location.origin}${url}`, '_blank').setInherit());
                    });
            }
        } else {
            this.setTitle(`New ${contentType.name}`);
        }

        formBuilder.fieldModels.then(fieldModels => {
            if (fieldModels.length == 0) {

                var image = `<img class="poetry-ui-help-illustration" src="${window.staticFilesBasePath}/ContentAppSupport/images/undraw_order_a_car_3tww.svg" alt="Illustration of a house with cars surrounding it, bearing checkmarks.">`;
                var header1 = `<h2 class="poetry-ui-help-heading">No properties</h2>`;
                var text1 = '<p>You should probably define some kind of properties here.</p>';
                var text2 = '<p>Some helpful topics:</p>';

                var helpList = new List();
                helpList.addItem(item => item.setText('I want to make my content translateable'));
                helpList.addItem(item => item.setText('I want to make my content navigatable with a browser'));
                helpList.addItem(item => item.setText('I want to support sub pages'));
                helpList.addItem(item => item.setText('I want to use a text area'));
                helpList.addItem(item => item.setText('I want to upload images'));
                helpList.addItem(item => item.setText('I want to create links to other content'));
                helpList.addItem(item => item.setText('I want to have lists of custom objects'));
                helpList.addItem(item => item.setText('I want to reuse several properties between content types'));

                var helpContainer = document.createElement('poetry-ui-help-container');
                helpContainer.innerHTML = image + header1 + text1 + text2;
                helpContainer.append(helpList.element);
                this.setContent(helpContainer);

                return;
            }

            var groups = [...new Set(fieldModels.map(fieldModel => fieldModel.descriptor.group))].sort();
            
            if (groups.length == 1) {
                formBuilder.build(content, { group: groups[0] }).then(form => this.setContent(form));

                return;
            }

            var tabSystem = new TabSystem();

            if (groups.indexOf(null) != -1) {
                tabSystem.addTab('General', () => {
                    var element = document.createElement('div');
                    formBuilder.build(content, { group: null }).then(form => form.appendTo(element));
                    return element;
                });
            }

            groups.filter(g => g != null).forEach(group => tabSystem.addTab(group, () => {
                var element = document.createElement('div');
                formBuilder.build(content, { group: group }).then(form => form.appendTo(element));
                return element;
            }));

            this.setContent(tabSystem);
        });

        var saveButton = new Button('Save')
            .setPrimary()
            .onClick(() =>
                fetch('ContentApp/SaveContent', {
                    credentials: 'include',
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        id: content.id,
                        contentTypeId: contentType.id,
                        content: JSON.stringify(content)
                    })
                })
                    .then(() => this.onSaveCallbacks.forEach(callback => callback(content)))
                    .then(() => {
                        if (!content.id) {
                            app.close(this);
                        }
                    })
            );
        var cancelButton = new Button('Cancel').onClick(() => app.close(this));
        var paste = text => {
            const value = JSON.parse(text);

            for (let [propertyKey, propertyValue] of Object.entries(content)) {
                if (propertyKey == 'id') {
                    continue;
                }

                if (propertyKey == 'contentTypeId') {
                    continue;
                }

                if (propertyKey == 'language') {
                    continue;
                }

                if (!(propertyKey in value)) {
                    delete content[propertyKey];
                }
            }
            for (let [propertyKey, propertyValue] of Object.entries(value)) {
                if (propertyKey == 'id') {
                    continue;
                }

                if (propertyKey == 'contentTypeId') {
                    continue;
                }

                if (propertyKey == 'language') {
                    continue;
                }

                content[propertyKey] = propertyValue;
            }
        };
        var moreButton = new ContextMenu()
            .addItem(item => item.setText('Copy').onClick(() => navigator.clipboard.writeText(JSON.stringify(content, null, '  '))))
            .addItem(item => item.setText('Paste').onClick(() => { app.closeAfter(this); navigator.clipboard.readText().then(paste); }));

        this.setFooter(saveButton, cancelButton, moreButton);
    }

    onSave(callback) {
        this.onSaveCallbacks.push(callback);

        return this;
    }
}