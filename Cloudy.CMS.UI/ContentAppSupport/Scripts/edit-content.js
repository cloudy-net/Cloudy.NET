import App from '../../../Poetry.UI/Scripts/app.js';
import Blade from '../../../Poetry.UI/Scripts/blade.js';
import FormBuilder from '../../../Poetry.UI.FormSupport/Scripts/form-builder.js';
import Button from '../../../Poetry.UI/Scripts/button.js';
import DataTable from '../../../Poetry.UI.DataTableSupport/Scripts/data-table.js';
import Backend from '../../../Poetry.UI.DataTableSupport/Scripts/backend.js';
import CopyAsTabSeparated from '../../../Poetry.UI.DataTableSupport/Scripts/copy-as-tab-separated.js';
import ContextMenu from '../../../Poetry.UI.ContextMenuSupport/Scripts/context-menu.js';
import notificationManager from '../../../Poetry.UI.NotificationSupport/Scripts/notification-manager.js';



/* EDIT CONTENT */

class EditContentBlade extends Blade {
    constructor(app, contentType, item) {
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
            if (item) {
                if (contentType.IsNameable && item.Name) {
                    this.setTitle(`Edit ${item.Name}`);
                } else {
                    this.setTitle(`Edit ${contentType.Name}`);
                }

                if (contentType.IsRoutable) {
                    fetch(`Cloudy.CMS.UI/ContentApp/GetUrl?id=${encodeURIComponent(item.Id)}&contentTypeId=${encodeURIComponent(item.ContentTypeId)}`, {
                        credentials: 'include',
                        method: 'GET',
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    })
                        .then(response => response.json())
                        .then(url => {
                            view.href = `${location.origin}/${url}`;
                            view.removeAttribute('disabled');
                        });
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
                        new Button('Edit').onClick(() => app.openBlade(new EditPropertyGroupBlade(app, formBuilder.build(item, { group: group }), group || 'Content'), this))
                    ))
            );
        };

        if (item instanceof Promise) {
            item.then(item => init(item));
        } else if (item) {
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