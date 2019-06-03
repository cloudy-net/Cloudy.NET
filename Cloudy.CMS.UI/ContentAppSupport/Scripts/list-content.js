import App from '../../../Poetry.UI/Scripts/app.js';
import Blade from '../../../Poetry.UI/Scripts/blade.js';
import FormBuilder from '../../../Poetry.UI.FormSupport/Scripts/form-builder.js';
import Button from '../../../Poetry.UI/Scripts/button.js';
import DataTable from '../../../Poetry.UI.DataTableSupport/Scripts/data-table.js';
import Backend from '../../../Poetry.UI.DataTableSupport/Scripts/backend.js';
import CopyAsTabSeparated from '../../../Poetry.UI.DataTableSupport/Scripts/copy-as-tab-separated.js';
import ContextMenu from '../../../Poetry.UI.ContextMenuSupport/Scripts/context-menu.js';
import notificationManager from '../../../Poetry.UI.NotificationSupport/Scripts/notification-manager.js';
import EditContentBlade from './edit-content.js';



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

export default ListContentBlade;