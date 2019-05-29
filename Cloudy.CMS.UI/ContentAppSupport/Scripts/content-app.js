import App from '../../../Poetry.UI/Scripts/app.js';
import Blade from '../../../Poetry.UI/Scripts/blade.js';
import FormBuilder from '../../../Poetry.UI.FormSupport/Scripts/form-builder.js';
import Button from '../../../Poetry.UI/Scripts/button.js';
import DataTable from '../../../Poetry.UI.DataTableSupport/Scripts/data-table.js';
import Backend from '../../../Poetry.UI.DataTableSupport/Scripts/backend.js';
import CopyAsTabSeparated from '../../../Poetry.UI.DataTableSupport/Scripts/copy-as-tab-separated.js';
import ContextMenu from '../../../Poetry.UI.ContextMenuSupport/Scripts/context-menu.js';
import notificationManager from '../../../Poetry.UI.NotificationSupport/Scripts/notification-manager.js';



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
                .addColumn(c => c.setHeader(() => 'Name').setContent(item => item.IsSingleton ? item.Name : item.PluralName))
                .addColumn(c => c.setActionColumn().setContent(item =>
                    item.IsSingleton ?
                        new Button('Edit').onClick(() =>
                            app.openBlade(new EditContentBlade(app, item, {
                                itemPromise:
                                    fetch(`Cloudy.CMS.UI/ContentApp/GetSingleton?id=${item.SingletonId}`, {
                                        credentials: 'include',
                                        method: 'Get',
                                    })
                                        .then(response => response.json())
                            }), this)) :
                        new Button('List').onClick(() => app.openBlade(new ListContentBlade(app, item), this))
                ))
        );
    }
}



/* LIST CONTENT BLADE */

class ListContentBlade extends Blade {
    constructor(app, contentType) {
        super();

        this.setTitle(contentType.PluralName);

        var dataTable = new DataTable()
            .setBackend(`Cloudy.CMS.ContentList[type=${contentType.Id}]`);

        if (contentType.IsNameable) {
            dataTable.addColumn(c => c.setHeader(() => 'Name').setContent(item => item.Name));
        } else {
            dataTable.addColumn(c => c.setHeader(() => 'Id').setContent(item => item.Id));  
        }

        dataTable.addColumn(c => c.setActionColumn().setContent(item => new Button('Edit').onClick(() => app.openBlade(new EditContentBlade(app, contentType, item).onClose(message => { if (message == 'saved') { dataTable.update(); } }), this))))

        this.setToolbar(
            new Button('New').onClick(() => app.openBlade(new EditContentBlade(app, contentType).onClose(message => { if (message == 'saved') { dataTable.update(); } }), this))
        );

        this.setContent(dataTable);
    }
}



/* EDIT CONTENT */

class EditContentBlade extends Blade {
    constructor(app, contentType, options) {
        super();

        this.setTitle();
        this.setContent();

        var saveButton = new Button('Save').setDisabled();
        var cancelButton = new Button('Cancel').onClick(() => app.closeBlade(this)).setDisabled();

        this.setToolbar(saveButton, cancelButton);

        var init = item => {
            if (item) {
                if (contentType.IsNameable && item.Name) {
                    this.setTitle(`Edit ${item.Name}`);
                } else {
                    this.setTitle(`Edit ${contentType.Name}`);
                }
            } else {
                this.setTitle(`New ${contentType.Name}`);
            }

            var save = () =>
                fetch('Cloudy.CMS.UI/ContentApp/Save', {
                    credentials: 'include',
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        Id: item.Id,
                        ContentTypeId: contentType.Id,
                        item: item
                    })
                });

            saveButton.onClick(() => save().then(() => app.closeBlade(this, 'saved'))).setDisabled(false);
            cancelButton.setDisabled(false);

            var formBuilder = new FormBuilder(`Cloudy.CMS.Content[type=${contentType.Id}]`, app);

            var backend = new class extends Backend {
                load(query) {
                    return formBuilder.fieldModels.then(fieldModels => {
                        var items = new Set(fieldModels.map(fieldModel => fieldModel.descriptor.Group));

                        return {
                            Items: items,
                            PageCount: 1,
                            PageSize: items.size,
                            TotalMatching: items.size,
                        };
                    });
                }
            };

            this.setContent(
                new DataTable()
                    .setBackend(backend)
                    .addColumn(c => c.setHeader(() => 'Properties').setContent(item => item || 'Content'))
                    .addColumn(c => c.setActionColumn().setContent(group =>
                        new Button('Edit').onClick(() => app.openBlade(new EditPropertyGroupBlade(app, formBuilder.build(item, { group: group }), group || 'Content').onClose((message, values) => { if (message == 'saved') { console.log(values); } }), this))
                    ))
            );
        };

        if (options.itemPromise) {
            options.itemPromise.then(item => init(item));
        } else if (options.item) {
            init(options.item);
        } else {
            init({});
        }
    }
}



/* EDIT PROPERTY GROUP */

class EditPropertyGroupBlade extends Blade {
    constructor(app, formPromise, title) {
        super();

        this.setTitle(title);

        this.setContent();

        var saveButton = new Button('Save');

        this.setToolbar(
            saveButton,
            new Button('Cancel').onClick(() => app.closeBlade(this)),
        );

        formPromise.then(form => {
            this.setContent(form);
            saveButton.onClick(() => app.closeBlade(this, 'saved', form.getValues()));
        });
    }
}