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

        if (content.id) {
            if (contentType.isNameable && content.name) {
                this.setTitle(`Edit ${content.name}`);
            } else {
                this.setTitle(`Edit ${contentType.name}`);
            }

            if (contentType.isRoutable) {
                fetch(`Content/GetUrl?id=${encodeURIComponent(content.id)}&contentTypeId=${encodeURIComponent(content.contentTypeId)}`, {
                    credentials: 'include',
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .catch(error => notificationManager.addNotification(item => item.setText(`Could not get URL (${error.name}: ${error.message})`)))
                    .then(response => response.text())
                    .then(url => {
                        if (!url) {
                            return;
                        }

                        url = url.substr(1, url.length - 2);

                        this.setToolbar(new LinkButton('View', `${location.origin}${url}`, '_blank').setInherit());
                    });
            }
        } else {
            this.setTitle(`New ${contentType.name}`);
        }

        formBuilder.fieldModels.then(fieldModels => {
            if (fieldModels.length == 0) {

                var image = `<img class="poetry-ui-help-illustration" src="${window.staticFilesBasePath}/ContentAppSupport/images/undraw_order_a_car_3tww.svg" alt="Illustration of a house with cars surrounding it, bearing checkmarks.">`;
                var header1 = `<h2 class="poetry-ui-help-heading">No properties</h2>`;
                var text1 = '<p>You should probably define some kind of properties here.</p>';
                var text2 = '<p>Some helpful topics:</p>';

                var helpList = new List();
                helpList.addItem(item => item.setText('I want to make my content translateable'));
                helpList.addItem(item => item.setText('I want to make my content navigatable with a browser'));
                helpList.addItem(item => item.setText('I want to support sub pages'));
                helpList.addItem(item => item.setText('I want to use a text area'));
                helpList.addItem(item => item.setText('I want to upload images'));
                helpList.addItem(item => item.setText('I want to create links to other content'));
                helpList.addItem(item => item.setText('I want to have lists of custom objects'));
                helpList.addItem(item => item.setText('I want to reuse several properties between content types'));

                var helpContainer = document.createElement('poetry-ui-help-container');
                helpContainer.innerHTML = image + header1 + text1 + text2;
                helpContainer.append(helpList.element);
                this.setContent(helpContainer);

                return;
            }

            var groups = [...new Set(fieldModels.map(fieldModel => fieldModel.descriptor.group))].sort();

            if (groups.length == 1) {
                formBuilder.build(content, { group: groups[0] }).then(form => this.setContent(form));

                return;
            }

            var tabSystem = new TabSystem();

            if (groups.indexOf(null) != -1) {
                tabSystem.addTab('General', () => {
                    var element = document.createElement('div');
                    formBuilder.build(content, { group: null }).then(form => form.appendTo(element));
                    return element;
                });
            }

            groups.filter(g => g != null).forEach(group => tabSystem.addTab(group, () => {
                var element = document.createElement('div');
                formBuilder.build(content, { group: group }).then(form => form.appendTo(element));
                return element;
            }));

            this.setContent(tabSystem);
        });

        var saveButton = new Button('Save')
            .setPrimary()
            .onClick(() =>
                fetch('Content/SaveContent', {
                    credentials: 'include',
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        id: content.id,
                        contentTypeId: contentType.id,
                        content: JSON.stringify(content)
                    })
                })
                    .catch(error => notificationManager.addNotification(item => item.setText(`Could not save content (${error.name}: ${error.message})`)))
                    .then(() => this.onCompleteCallbacks.forEach(callback => callback(content)))
                    .then(() => {
                        var name;

                        if (contentType.isNameable) {
                            name = contentType.nameablePropertyName ? content[contentType.nameablePropertyName] : content.name;

                            if (!name) {
                                name = content.id || ''; // if content is newly created, id will still be null
                            }
                        } else {
                            name = content.id;
                        }

                        if (!content.id) {
                            notificationManager.addNotification(item => item.setText(`Created ${contentType.name} ${name}`));
                            app.close(this);
                        } else {
                            notificationManager.addNotification(item => item.setText(`Updated ${contentType.name} ${name}`));
                        }
                    })
            );
        var cancelButton = new Button('Cancel').onClick(() => app.close(this));
        var paste = text => {
            const value = JSON.parse(text);

            for (let [propertyKey, propertyValue] of Object.entries(content)) {
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
                    delete content[propertyKey];
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

                content[propertyKey] = propertyValue;
            }
        };
        var moreButton = new ContextMenu()
            .addItem(item => item.setText('Copy').onClick(() => navigator.clipboard.writeText(JSON.stringify(content, null, '  '))))
            .addItem(item => item.setText('Paste').onClick(() => { app.closeAfter(this); navigator.clipboard.readText().then(paste); }));

        this.setFooter(saveButton, cancelButton, moreButton);
    }

    onComplete(callback) {
        this.onCompleteCallbacks.push(callback);

        return this;
    }
}

export default EditContentBlade;