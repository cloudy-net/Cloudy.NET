import App from '../../../Poetry.UI/Scripts/app.js';
import Blade from '../../../Poetry.UI/Scripts/blade.js';
import FormBuilder from '../../../Poetry.UI.FormSupport/Scripts/form-builder.js';
import Button from '../../../Poetry.UI/Scripts/button.js';
import DataTable from '../../../Poetry.UI.DataTableSupport/Scripts/data-table.js';
import DataTableButton from '../../../Poetry.UI.DataTableSupport/Scripts/data-table-button.js';
import Backend from '../../../Poetry.UI.DataTableSupport/Scripts/backend.js';
import CopyAsTabSeparated from '../../../Poetry.UI.DataTableSupport/Scripts/copy-as-tab-separated.js';
import ContextMenu from '../../../Poetry.UI.ContextMenuSupport/Scripts/context-menu.js';
import notificationManager from '../../../Poetry.UI.NotificationSupport/Scripts/notification-manager.js';
import ListContentBlade from './list-content.js';
import EditContentBlade from './edit-content.js';



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
                .addColumn(c => c.setHeader(element => 'Name').setButton(item => {
                    var name = item.IsSingleton ? item.Name : item.PluralName;
                    var button = new DataTableButton(name).onClick(() => button.setActive());

                    if (item.IsSingleton) {
                        button.onClick(() =>
                            app.openBlade(
                                new EditContentBlade(
                                    app,
                                    item,
                                    fetch(`Cloudy.CMS.UI/ContentApp/GetSingleton?id=${item.Id}`, {
                                        credentials: 'include',
                                        method: 'Get',
                                    }).then(response => response.json())
                                ).onClose(() => button.setActive(false)),
                                this
                            )
                        );
                    } else {
                        button.onClick(() =>
                            app.openBlade(new ListContentBlade(app, item).onClose(() => button.setActive(false)), this)
                        );
                    }

                    return button;
                }))
        );
    }
}