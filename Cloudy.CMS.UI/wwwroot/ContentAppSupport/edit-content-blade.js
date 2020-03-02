import Blade from '../blade.js';
import Button from '../button.js';
import LinkButton from '../link-button.js';
import ContextMenu from '../ContextMenuSupport/context-menu.js';
import List from '../ListSupport/list.js';
import TabSystem from '../TabSupport/tab-system.js';
import notificationManager from '../NotificationSupport/notification-manager.js';



/* EDIT CONTENT */

class EditContentBlade extends Blade {
    onCompleteCallbacks = [];

    constructor(app, contentType, formBuilder, content) {
        super();

        if (!content) {
            content = {};
        }

        this.app = app;
        this.contentType = contentType;
        this.formBuilder = formBuilder;
        this.content = content;
    }

    async open() {
        if (this.content.id) {
            if (this.contentType.isNameable && this.content.name) {
                this.setTitle(`Edit ${this.content.name}`);
            } else {
                this.setTitle(`Edit ${this.contentType.name}`);
            }

            if (this.contentType.isRoutable) {
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

                if (!url) {
                    return;
                }

                url = url.substr(1, url.length - 2);

                this.setToolbar(new LinkButton('View', `${location.origin}${url}`, '_blank').setInherit());
            }
        } else {
            this.setTitle(`New ${this.contentType.name}`);
        }

        var fieldModels = await this.formBuilder.fieldModels;
        
        if (fieldModels.length == 0) {

            //var image = `<img class="cloudy-ui-help-illustration" src="${window.staticFilesBasePath}/ContentAppSupport/images/undraw_order_a_car_3tww.svg" alt="Illustration of a house with cars surrounding it, bearing checkmarks.">`;
            //var header1 = `<h2 class="cloudy-ui-help-heading">No properties</h2>`;
            //var text1 = '<p>You should probably define some kind of properties here.</p>';
            //var text2 = '<p>Some helpful topics:</p>';

            //var helpList = new List();
            //helpList.addItem(item => item.setText('I want to make my content translateable'));
            //helpList.addItem(item => item.setText('I want to make my content navigatable with a browser'));
            //helpList.addItem(item => item.setText('I want to support sub pages'));
            //helpList.addItem(item => item.setText('I want to use a text area'));
            //helpList.addItem(item => item.setText('I want to upload images'));
            //helpList.addItem(item => item.setText('I want to create links to other content'));
            //helpList.addItem(item => item.setText('I want to have lists of custom objects'));
            //helpList.addItem(item => item.setText('I want to reuse several properties between content types'));

            //var helpContainer = document.createElement('cloudy-ui-help-container');
            //helpContainer.innerHTML = image + header1 + text1 + text2;
            //helpContainer.append(helpList.element);
            //this.setContent(helpContainer);

            //return;
        }

        var groups = [...new Set(fieldModels.map(fieldModel => fieldModel.descriptor.group))].sort();

        if (groups.length == 1) {
            var form = await this.formBuilder.build(this.content, { group: groups[0] });

            this.setContent(form);

            return;
        }

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

        var saveButton = new Button('Save')
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
                        throw new Error(`${response.status} (${response.statusText})`);
                    }

                    var result = await response.json();
                } catch (error) {
                    notificationManager.addNotification(item => item.setText(`Could not save content (${error.name}: ${error.message})`));
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

                var name;

                if (this.contentType.isNameable) {
                    name = this.contentType.nameablePropertyName ? this.content[this.contentType.nameablePropertyName] : this.content.name;

                    if (!name) {
                        name = this.content.id || ''; // if content is newly created, id will still be null
                    }
                } else {
                    name = this.content.id;
                }

                if (!this.content.id) {
                    notificationManager.addNotification(item => item.setText(`Created ${this.contentType.name} ${name}`));
                    this.app.close(this);
                } else {
                    notificationManager.addNotification(item => item.setText(`Updated ${this.contentType.name} ${name}`));
                }

                this.onCompleteCallbacks.forEach(callback => callback(this.content));
            });

        var cancelButton = new Button('Cancel').onClick(() => this.app.close(this));
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
        };
        var moreButton = new ContextMenu()
            .addItem(item => item.setText('Copy').onClick(() => navigator.clipboard.writeText(JSON.stringify(this.content, null, '  '))))
            .addItem(item => item.setText('Paste').onClick(() => { this.app.closeAfter(this); navigator.clipboard.readText().then(paste); }));

        this.setFooter(saveButton, cancelButton, moreButton);
    }

    onComplete(callback) {
        this.onCompleteCallbacks.push(callback);

        return this;
    }
}

export default EditContentBlade;