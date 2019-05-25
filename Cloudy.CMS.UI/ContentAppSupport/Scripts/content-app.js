import App from '../../../Poetry.UI/Scripts/app.js';
import Blade from '../../../Poetry.UI/Scripts/blade.js';
import FormBuilder from '../../../Poetry.UI.FormSupport/Scripts/form-builder.js';
import translationRepository from '../../../Poetry.UI/Scripts/translation-repository.js';
import Button from '../../../Poetry.UI/Scripts/button.js';
import DataTable from '../../../Poetry.UI.DataTableSupport/Scripts/data-table.js';
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
                .addColumn(c => c.setHeader(() => 'Name').setContent(item => item.Name))
                .addColumn(c => c.setActionColumn().setContent(item => new Button('List').onClick(() => app.openBlade(new ListContentBlade(app, item), this))))
        );
    }
}



/* LIST CONTENT BLADE */

class ListContentBlade extends Blade {
    constructor(app, contentType) {
        super();

        this.setTitle(contentType.Name);

        var dataTable = new DataTable()
            .setBackend(`Cloudy.CMS.ContentList[type=${contentType.Id}]`);

        if (contentType.IsNameable) {
            dataTable.addColumn(c => c.setHeader(() => 'Name').setContent(item => item.Name));
        } else {
            dataTable.addColumn(c => c.setHeader(() => 'Id').setContent(item => item.Id));  
        }

        dataTable.addColumn(c => c.setActionColumn().setContent(item => new Button('Edit').onClick(() => app.openBlade(new EditContentBlade(app, contentType, item), this))))

        this.setToolbar(
            new Button('New').onClick(() => app.openBlade(new EditContentBlade(app, contentType).onClose(message => { if (message == 'saved') { dataTable.update(); } }), this))
        );

        this.setContent(dataTable);
    }
}



/* EDIT */

class EditContentBlade extends Blade {
    constructor(app, contentType, item) {
        super();

        if (item) {
            if (contentType.IsNameable && item.Name) {
                this.setTitle(`Edit ${item.Name}`);
            } else {
                this.setTitle(`Edit ${contentType.Name}`);
            }
        } else {
            this.setTitle(`New ${contentType.Name}`);
            item = {};
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

        this.setContent();

        this.setToolbar(
            new Button('Save').onClick(() => save().then(() => app.closeBlade(this, 'saved'))),
            new Button('Cancel').onClick(() => app.closeBlade(this)),
        );

        new FormBuilder(`Cloudy.CMS.Content[type=${contentType.Id}]`, app).build(item).then(form => this.setContent(form.element));
    }
}