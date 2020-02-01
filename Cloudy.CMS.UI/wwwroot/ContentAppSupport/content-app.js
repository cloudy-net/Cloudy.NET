import App from '../app.js';
import Blade from '../blade.js';
import FormBuilder from '../FormSupport/form-builder.js';
import Button from '../button.js';
import DataTable from '../DataTableSupport/data-table.js';
import DataTableButton from '../DataTableSupport/data-table-button.js';
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

class ListContentTypesBlade extends Blade {
    constructor(app) {
        super();

        this.setTitle('Content types');

        fetch('ContentApp/GetContentTypes', { credentials: 'include' })
            .then(response => response.json())
            .then(contentTypes => {
                var list = new List();
                contentTypes.forEach(contentType => list.addItem(item => {
                    item.setText(contentType.name);

                    if (contentType.isSingleton) {
                        item.setSubText('Singleton');
                        
                        var formBuilder = new FormBuilder(`Cloudy.CMS.Content[type=${contentType.id}]`, app);
                        var content = fetch(`ContentApp/GetSingleton?id=${contentType.id}`, { credentials: 'include' }).then(response => response.json());

                        Promise.all([formBuilder.fieldModels, content]).then(results =>
                            item.onClick(() => {
                                item.setActive();
                                app.openAfter(
                                    new EditContentBlade(app, contentType, formBuilder, results[1])
                                        .onClose(() => item.setActive(false)),
                                    this);
                            })
                        );
                    } else {
                        item.onClick(() => {
                            item.setActive();
                            app.openAfter(new ListContentBlade(app, contentType).onClose(() => item.setActive(false)), this);
                        });
                    }
                }));
                this.setContent(list);
            });
    }
}



/* LIST CONTENT BLADE */

class ListContentBlade extends Blade {
    constructor(app, contentType) {
        super();

        this.setTitle(contentType.pluralName);

        var formBuilder = new FormBuilder(`Cloudy.CMS.Content[type=${contentType.id}]`, app);

        fetch(`ContentApp/GetContentList?contentTypeId=${contentType.id}`, { credentials: 'include' })
            .then(response => response.json())
            .then(response => {
                var list = new List();
                response.forEach(content => list.addItem(item => {
                    item.setText(contentType.isNameable ? content.name : content.id);

                    formBuilder.fieldModels.then(fieldModels => item.onClick(() => app.openAfter(new EditContentBlade(app, contentType, formBuilder, content).onSave(() => dataTable.update()), this)));
                }));
                this.setContent(list);
            });

        this.setToolbar(
            new Button('New').setInherit().onClick(() =>
                formBuilder.fieldModels.then(fieldModels => app.openAfter(new EditContentBlade(app, contentType, formBuilder).onSave(() => dataTable.update()), this))
            )
        );
    }
}



/* EDIT CONTENT */

class EditContentBlade extends Blade {
    onSaveCallbacks = [];

    constructor(app, contentType, formBuilder, content) {
        super();

        if (contentType.IsRoutable) {
            var view = document.createElement('a');
            view.classList.add('poetry-ui-button');
            view.setAttribute('disabled', true);
            view.setAttribute('target', '_blank');
            view.innerText = 'View';
            this.setHeader(view);
        }

        if (!content) {
            content = {};
        }

        if (content.Id) {
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
                        view.href = `${location.origin}${url}`;
                        view.removeAttribute('disabled');
                    });
            }
        } else {
            this.setTitle(`New ${contentType.name}`);
        }

        formBuilder.fieldModels.then(fieldModels => {
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
                fetch('ContentApp/Save', {
                    credentials: 'include',
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        id: content.id,
                        contentTypeId: contentType.id,
                        item: content
                    })
                })
                    .then(() => this.onSaveCallbacks.forEach(callback => callback(content)))
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