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
                    var name = contentType.isSingleton ? contentType.name : contentType.pluralName;
                    var button = new DataTableButton(name).onClick(() => button.setActive());

                    if (contentType.isSingleton) {
                        var formBuilder = new FormBuilder(`Cloudy.CMS.Content[type=${contentType.id}]`, app);
                        var item = fetch(`Cloudy.CMS.UI/ContentApp/GetSingleton?id=${contentType.id}`, {
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