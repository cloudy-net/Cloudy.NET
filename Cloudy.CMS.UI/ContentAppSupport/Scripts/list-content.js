import App from '../../../Cloudy.CMS.Admin/Scripts/app.js';
import Blade from '../../../Cloudy.CMS.Admin/Scripts/blade.js';
import FormBuilder from '../../../Cloudy.CMS.Admin/FormSupport/Scripts/form-builder.js';
import Button from '../../../Cloudy.CMS.Admin/Scripts/button.js';
import DataTable from '../../../Cloudy.CMS.Admin/DataTableSupport/Scripts/data-table.js';
import DataTableButton from '../../../Cloudy.CMS.Admin/DataTableSupport/Scripts/data-table-button.js';
import Backend from '../../../Cloudy.CMS.Admin/DataTableSupport/Scripts/backend.js';
import CopyAsTabSeparated from '../../../Cloudy.CMS.Admin/DataTableSupport/Scripts/copy-as-tab-separated.js';
import ContextMenu from '../../../Cloudy.CMS.Admin/ContextMenuSupport/Scripts/context-menu.js';
import notificationManager from '../../../Cloudy.CMS.Admin/NotificationSupport/Scripts/notification-manager.js';
import EditContentBlade from './edit-content.js';



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
                        app.openBlade(new EditContentBlade(app, contentType, formBuilder, item).onClose(message => { if (message == 'saved') { dataTable.update(); } }).onClose(() => button.setActive(false)), this);
                    })
                );

                return button;
            })
        );

        this.setToolbar(
            new Button('New').onClick(() => app.openBlade(new EditContentBlade(app, contentType).onClose(message => { if (message == 'saved') { dataTable.update(); } }), this))
        );

        this.setContent(dataTable);
    }
}

export default ListContentBlade;