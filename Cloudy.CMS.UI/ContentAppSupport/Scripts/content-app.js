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
                .addColumn(c => c.setHeader(element => 'Name').setButton(contentType => {
                    var name = contentType.IsSingleton ? contentType.Name : contentType.PluralName;
                    var button = new DataTableButton(name).onClick(() => button.setActive());

                    if (contentType.IsSingleton) {
                        var formBuilder = new FormBuilder(`Cloudy.CMS.Content[type=${contentType.Id}]`, app);
                        var item = fetch(`Cloudy.CMS.UI/ContentApp/GetSingleton?id=${contentType.Id}`, {
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