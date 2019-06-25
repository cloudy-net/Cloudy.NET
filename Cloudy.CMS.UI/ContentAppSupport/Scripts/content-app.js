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
    open() {
        this.openBlade(new ListContentTypesBlade(this));
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
                        var item = fetch(`ContentApp/GetSingleton?id=${contentType.id}`, {
                            credentials: 'include',
                            method: 'Get',
                        })
                            .then(response => response.json());

                        Promise.all([formBuilder.fieldModels, item]).then(results =>
                            button.onClick(() =>
                                app.openBlade(
                                    new EditContentBlade(
                                        app,
                                        contentType,
                                        formBuilder,
                                    ).onClose(() => button.setActive(false)),
                                    this
                                )
                            )
                        );
                    } else {
                        button.onClick(() =>
                            app.openBlade(new ListContentBlade(app, contentType).onClose(() => button.setActive(false)), this)
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
                        app.openBlade(new EditContentBlade(app, contentType, formBuilder, item).onSave(() => dataTable.update()).onClose(() => button.setActive(false)), this);
                    })
                );

                return button;
            })
        );

        this.setToolbar(
            new Button('New').onClick(() =>
                formBuilder.fieldModels.then(fieldModels =>
                    app.openBlade(new EditContentBlade(app, contentType, formBuilder).onSave(() => dataTable.update()), this)
                )
            )
        );

        this.setContent(dataTable);
    }
}



/* EDIT CONTENT */

class EditContentBlade extends Blade {
    onSaveCallbacks = [];

    constructor(app, contentType, formBuilder, item) {
        super();

        this.setTitle();
        if (contentType.IsRoutable) {
            var view = document.createElement('a');
            view.classList.add('poetry-ui-button');
            view.setAttribute('disabled', true);
            view.setAttribute('target', '_blank');
            view.innerText = 'View';
            this.setToolbar(view);
        }

        var saveButton = new Button('Save').setDisabled();
        var cancelButton = new Button('Cancel').onClick(() => app.closeBlade(this)).setDisabled();

        this.setFooter(saveButton, cancelButton);

        var init = item => {
            if (item.Id) {
                if (contentType.isNameable && item.name) {
                    this.setTitle(`Edit ${item.name}`);
                } else {
                    this.setTitle(`Edit ${contentType.name}`);
                }

                if (contentType.isRoutable) {
                    fetch(`ContentApp/GetUrl?id=${encodeURIComponent(item.id)}&contentTypeId=${encodeURIComponent(item.contentTypeId)}`, {
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

            var save = () =>
                fetch('ContentApp/Save', {
                    credentials: 'include',
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        id: item.id,
                        contentTypeId: contentType.id,
                        item: item
                    })
                })
                .then(() => this.onSaveCallbacks.forEach(callback => callback(item)));

            saveButton.onClick(() => save()).setDisabled(false);
            cancelButton.setDisabled(false);

            formBuilder.fieldModels.then(fieldModels =>
                this.setContent(
                    new DataTable()
                        .setBackend([...new Set(fieldModels.map(fieldModel => fieldModel.descriptor.group))].sort())
                        .addColumn(c =>
                            c.setHeader(() => 'Properties').setButton(group => {
                                var button = new DataTableButton(group || 'General');

                                button.onClick(() => {
                                    button.setActive();
                                    app.openBlade(new EditPropertyGroupBlade(app, formBuilder.build(item, { group: group }), group || 'Content').onClose(() => button.setActive(false)), this)
                                });

                                return button;
                            })
                        )
                )
            );
        };

        if (item) {
            init(item);
        } else {
            init({});
        }
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
            new Button('Close').onClick(() => app.closeBlade(this)),
        );

        formPromise.then(form => this.setContent(form));
    }
}