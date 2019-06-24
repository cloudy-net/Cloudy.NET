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



/* EDIT CONTENT */

class EditContentBlade extends Blade {
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
                fetch('Cloudy.CMS.UI/ContentApp/Save', {
                    credentials: 'include',
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        Id: item.id,
                        ContentTypeId: contentType.id,
                        item: item
                    })
                });

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
}

export default EditContentBlade;



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