import FieldControl from '../field-control.js';
import ItemProvider from './select-item-provider.js';
import Button from '../../button.js';
import SelectItemPreview from './select-item-preview.js';
import ContextMenu from '../../ContextMenuSupport/context-menu.js';
import notificationManager from '../../NotificationSupport/notification-manager.js';



/* FILE UPLOAD */

class FileUploadControl extends FieldControl {
    constructor(fieldModel, value, app, blade) {
        super(document.createElement('cloudy-ui-select'));

        this.input = document.createElement('input');
        this.input.type = "file";
        this.input.style.display = 'none';
        this.input.addEventListener('change', async event => {
            var file = event.target.files[0];

            if (!file) {
                this.update(null);
                return;
            }

            var reader = new FileReader();
            reader.onerror = error => notificationManager.addNotification(item => item.setText(`Could not read file`));
            reader.onload = () => {
                var value = { name: file.name, type: file.type, data: reader.result.substr(reader.result.indexOf(',') + 1) };

                this.triggerChange(value);
                this.update(value);
            };
            reader.readAsDataURL(file);
        });
        this.element.append(this.input);

        this.empty = document.createElement('cloudy-ui-select-empty');
        var emptyText = document.createElement('cloudy-ui-select-empty-text');
        emptyText.innerText = '(none)';
        this.empty.append(emptyText);
        this.element.append(this.empty);

        this.preview = new SelectItemPreview().appendTo(this.element);

        this.update(value);

        new Button('Add').onClick(() => this.open()).appendTo(this.empty);

        this.menu = new ContextMenu();

        this.menu.addItem(item => item.setText('Replace').onClick(() => this.open()));

        if (!fieldModel.descriptor.isSortable) {
            this.menu.addItem(item => item.setText('Clear').onClick(() => {
                this.item = null;
                this.parents = [];
                this.triggerChange(null);
                this.update();
            }));
        }

        this.preview.setMenu(this.menu);
        this.preview.onClick(() => setTimeout(() => this.menu.toggle(), 1));

        this.onSet(value => {
            this.update(value);
        });
    }

    update(value) {
        if (!value) {
            this.preview.element.style.display = 'none';
            this.empty.style.display = '';

            return;
        }

        this.preview.element.style.display = '';
        this.empty.style.display = 'none';
        if (value.type.indexOf('image/') == 0) {
            this.preview.setImage(`data:${value.type};base64,${value.data}`);
        }
        var pathSegments = value.name.split('/');
        var filename = pathSegments[pathSegments.length - 1];
        var name = filename.split('.')[0];
        this.preview.setText(name);

        var sizeBytes = value.data.length;
        var size = sizeBytes > 1024 * 1024 ? Math.round(sizeBytes / (1024 * 1024)) + 'MB' : sizeBytes > 1024 ? Math.round(sizeBytes / 1024) + 'KB' : sizeBytes + 'B';
        var format = filename.split('.')[1].toUpperCase();

        this.preview.setSubText(`Format: ${format}, Size: ${size}`);
    }

    open() {
        this.input.click();
    }
}

export default FileUploadControl;