import App from '../../Scripts/app.js';
import Blade from '../../Scripts/blade.js';
import FormBuilder from '../../FormSupport/Scripts/form-builder.js';
import Button from '../../Scripts/button.js';
import DataTable from '../../DataTableSupport/Scripts/data-table.js';
import DataTableButton from '../../DataTableSupport/Scripts/data-table-button.js';
import Backend from '../../DataTableSupport/Scripts/backend.js';
import CopyAsTabSeparated from '../../DataTableSupport/Scripts/copy-as-tab-separated.js';
import ContextMenu from '../../ContextMenuSupport/Scripts/context-menu.js';
import notificationManager from '../../NotificationSupport/Scripts/notification-manager.js';



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

        this.setContent(
            new DataTable()
                .setBackend('Cloudy.CMS.ContentTypeList')
                .addColumn(c => c.setHeader(element => 'Name').setButton(contentType => {
                    var name = contentType.isSingleton ? contentType.name : contentType.pluralName;
                    var button = new DataTableButton(name).onClick(() => button.setActive());

                    if (contentType.isSingleton) {
                        var formBuilder = new FormBuilder(`Cloudy.CMS.Content[type=${contentType.id}]`, app);
                        var content = fetch(`ContentApp/GetSingleton?id=${contentType.id}`, {
                            credentials: 'include',
                            method: 'Get',
                        })
                            .then(response => response.json());

                        Promise.all([formBuilder.fieldModels, content]).then(results =>
                            button.onClick(() =>
                                app.openAfter(
                                    new EditContentBlade(
                                        app,
                                        contentType,
                                        formBuilder,
                                        results[1],
                                    ).onClose(() => button.setActive(false)),
                                    this
                                )
                            )
                        );
                    } else {
                        button.onClick(() =>
                            app.openAfter(new ListContentBlade(app, contentType).onClose(() => button.setActive(false)), this)
                        );
                    }

                    return button;
                }))
        );
    }
}



/* LIST CONTENT BLADE */

class ListContentBlade extends Blade {
    constructor(app, contentType) {
        super();

        this.setTitle(contentType.pluralName);

        var formBuilder = new FormBuilder(`Cloudy.CMS.Content[type=${contentType.id}]`, app);

        var dataTable = new DataTable().setBackend(`Cloudy.CMS.ContentList[type=${contentType.id}]`);

        dataTable.addColumn(c =>
            c.setHeader(element => contentType.isNameable ? 'name' : 'id').setButton(item => {
                var button = new DataTableButton(contentType.isNameable ? item.name : item.id);

                formBuilder.fieldModels.then(fieldModels =>
                    button.onClick(() => {
                        button.setActive();
                        app.openAfter(new EditContentBlade(app, contentType, formBuilder, item).onSave(() => dataTable.update()).onClose(() => button.setActive(false)), this);
                    })
                );

                return button;
            })
        );

        this.setToolbar(
            new Button('New').onClick(() =>
                formBuilder.fieldModels.then(fieldModels =>
                    app.openAfter(new EditContentBlade(app, contentType, formBuilder).onSave(() => dataTable.update()), this)
                )
            )
        );

        this.setContent(dataTable);
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
            this.setToolbar(view);
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

        formBuilder.fieldModels.then(fieldModels =>
            this.setContent(
                new DataTable()
                    .setBackend([...new Set(fieldModels.map(fieldModel => fieldModel.descriptor.group))].sort())
                    .addColumn(c =>
                        c.setHeader(() => 'Properties').setButton(group => {
                            var button = new DataTableButton(group || 'General');

                            button.onClick(() => {
                                button.setActive();
                                app.openAfter(new EditPropertyGroupBlade(app, formBuilder.build(content, { group: group }), group || 'Content').onClose(() => button.setActive(false)), this)
                            });

                            return button;
                        })
                    )
            )
        );

        var saveButton = new Button('Save')
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
        var moreButton = new ContextMenu()
            .addItem(item => item.setText('Copy').onClick(() => navigator.clipboard.writeText(JSON.stringify(content, null, '  '))))
            .addItem(item => item.setText('Paste').onClick(() => { app.closeAfter(this); navigator.clipboard.readText().then(text => content = JSON.parse(text)); }));

        this.setFooter(saveButton, cancelButton, moreButton);
    }

    onSave(callback) {
        this.onSaveCallbacks.push(callback);

        return this;
    }
}



/* EDIT PROPERTY GROUP */

class EditPropertyGroupBlade extends Blade {
    constructor(app, formPromise, title) {
        super();

        this.setTitle(title);

        this.setFooter(
            new Button('Close').onClick(() => app.close(this)),
        );

        formPromise.then(form => this.setContent(form));
    }
}