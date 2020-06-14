import Blade from '../blade.js';
import Button from '../button.js';
import LinkButton from '../link-button.js';
import ContextMenu from '../ContextMenuSupport/context-menu.js';
import TabSystem from '../TabSupport/tab-system.js';
import notificationManager from '../NotificationSupport/notification-manager.js';
import FormBuilder from '../FormSupport/form-builder.js';
import fieldDescriptorProvider from '../FormSupport/field-descriptor-provider.js';



/* EDIT CONTENT */

class EditContentBlade extends Blade {
    onCompleteCallbacks = [];

    constructor(app, contentType, content) {
        super();

        if (!content) {
            content = {};
        }

        this.app = app;
        this.contentType = contentType;
        this.content = content;
        this.formId = `Cloudy.CMS.Content[type=${this.contentType.id}]`;

        this.element.addEventListener("keydown", (event) => {
            if ((String.fromCharCode(event.which).toLowerCase() == 's' && event.ctrlKey) || event.which == 19) { // 19 for Mac:s "Command+S"
                if (this.saveButton) {
                    this.saveButton.triggerClick();
                }
                event.preventDefault();
            }
        });
    }

    async open() {
        this.formBuilder = new FormBuilder(this.formId, this.app, this);

        if (this.content.id) {
            var name = '';

            if (!this.contentType.isSingleton) {
                if (this.contentType.isNameable) {
                    name = this.contentType.nameablePropertyName ? this.content[this.contentType.nameablePropertyName] : this.content.name;
                }
            }
            if (!name) {
                name = this.contentType.name;
            }

            this.setTitle(`Edit ${name}`);
        } else {
            this.setTitle(`New ${this.contentType.name}`);
        }

        if (this.content.id && this.contentType.isRoutable) {
            var response;

            try {
                response = await fetch(`Content/GetUrl?id=${encodeURIComponent(this.content.id)}&contentTypeId=${encodeURIComponent(this.content.contentTypeId)}`, {
                    credentials: 'include',
                    method: 'GET',
                    headers: { 'Content-Type': 'application/json' }
                });
            } catch (error) {
                notificationManager.addNotification(item => item.setText(`Could not get URL (${error.name}: ${error.message})`));
            }

            var url = await response.text();

            if (!url) {
                return;
            }

            url = url.substr(1, url.length - 2);

            this.setToolbar(new LinkButton('View', `${location.origin}${url}`, '_blank').setInherit());
        }

        this.buildForm();

        this.saveButton = new Button('Save')
            .setPrimary()
            .onClick(async () => {
                try {
                    var response = await fetch('Content/SaveContent', {
                        credentials: 'include',
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({
                            id: this.content.id,
                            contentTypeId: this.contentType.id,
                            content: JSON.stringify(this.content)
                        })
                    });

                    if (!response.ok) {
                        var text = await response.text();

                        if (text) {
                            throw new Error(text.split('\n')[0]);
                        } else {
                            text = response.statusText;
                        }

                        throw new Error(`${response.status} (${text})`);
                    }

                    var result = await response.json();
                } catch (error) {
                    notificationManager.addNotification(item => item.setText(`Could not save content (${error.message})`));
                }

                if (!result.success) {
                    var errors = document.createElement('ul');
                    Object.entries(result.validationErrors).forEach(error => {
                        var item = document.createElement('li');
                        item.innerText = `${error[0]}: ${error[1]}`;
                        errors.append(item);
                    });
                    notificationManager.addNotification(item => item.setText(`Error saving ${this.contentType.name}:`, errors));
                    return;
                }

                var name = null;

                if (!this.contentType.isSingleton) {
                    if (this.contentType.isNameable) {
                        name = this.contentType.nameablePropertyName ? this.content[this.contentType.nameablePropertyName] : this.content.name;
                    }
                    if (!name) {
                        name = this.content.id;
                    }
                }

                if (!this.content.id) {
                    notificationManager.addNotification(item => item.setText(`Created ${this.contentType.name} ${name || ''}`));
                    this.app.removeBlade(this);
                } else {
                    notificationManager.addNotification(item => item.setText(`Updated ${this.contentType.name} ${name || ''}`));
                }

                this.onCompleteCallbacks.forEach(callback => callback(this.content));
            });

        var cancelButton = new Button('Cancel').onClick(() => this.app.removeBlade(this));
        var paste = text => {
            const value = JSON.parse(text);

            for (let [propertyKey, propertyValue] of Object.entries(this.content)) {
                if (propertyKey == 'id') {
                    continue;
                }

                if (propertyKey == 'contentTypeId') {
                    continue;
                }

                if (propertyKey == 'language') {
                    continue;
                }

                if (!(propertyKey in value)) {
                    delete this.content[propertyKey];
                }
            }
            for (let [propertyKey, propertyValue] of Object.entries(value)) {
                if (propertyKey == 'id') {
                    continue;
                }

                if (propertyKey == 'contentTypeId') {
                    continue;
                }

                if (propertyKey == 'language') {
                    continue;
                }

                this.content[propertyKey] = propertyValue;
            }

            this.buildForm();
        };
        var moreButton = new ContextMenu()
            .addItem(item => item.setText('Copy').onClick(() => navigator.clipboard.writeText(JSON.stringify(this.content, null, '  '))))
            .addItem(item => item.setText('Paste').onClick(() => { this.app.removeBladesAfter(this); navigator.clipboard.readText().then(paste); }));

        this.setFooter(this.saveButton, cancelButton, moreButton);
    }

    async buildForm() {
        try {
            var groups = [...new Set((await fieldDescriptorProvider.getFor(this.formId)).map(fieldDescriptor => fieldDescriptor.group))].sort();

            if (groups.length == 1) {
                var form = await this.formBuilder.build(this.content, { group: groups[0] });

                this.setContent(form);
            } else {
                var tabSystem = new TabSystem();

                if (groups.indexOf(null) != -1) {
                    tabSystem.addTab('General', async () => {
                        var element = document.createElement('div');
                        var form = await this.formBuilder.build(this.content, { group: null });
                        form.appendTo(element);
                        return element;
                    });
                }

                groups.filter(g => g != null).forEach(group => tabSystem.addTab(group, async () => {
                    var element = document.createElement('div');
                    var form = await this.formBuilder.build(this.content, { group: group });
                    form.appendTo(element);
                    return element;
                }));

                this.setContent(tabSystem);
            }
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not build form --- ${error.message}`));
        }
    }

    onComplete(callback) {
        this.onCompleteCallbacks.push(callback);

        return this;
    }
}

export default EditContentBlade;