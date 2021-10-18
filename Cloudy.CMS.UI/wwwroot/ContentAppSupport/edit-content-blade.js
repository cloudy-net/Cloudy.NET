import Blade from '../blade.js';
import Button from '../button.js';
import TabSystem from '../TabSupport/tab-system.js';
import notificationManager from '../NotificationSupport/notification-manager.js';
import FormBuilder from '../FormSupport/form-builder.js';
import fieldDescriptorProvider from '../FormSupport/field-descriptor-provider.js';
import fieldModelBuilder from '../FormSupport/field-model-builder.js';
import primaryKeyProvider from './primary-key-provider.js';
import nameProvider from './name-provider.js';
import urlFetcher from '../url-fetcher.js';
import PopupMenu from '../PopupMenuSupport/popup-menu.js';

/* EDIT CONTENT */

class EditContentBlade extends Blade {
    onCompleteCallbacks = [];

    constructor(app, contentType, content) {
        super();
        
        this.app = app;
        this.contentType = contentType;
        this.content = content;
        this.contentId = primaryKeyProvider.getFor(this.content, this.contentType);
        this.formId = `Cloudy.CMS.Content[type=${this.contentType.id}]`;
        this.element.addEventListener("keydown", (event) => {
            if ((String.fromCharCode(event.which).toLowerCase() == 's' && event.ctrlKey) || event.which == 19) { // 19 for Mac:s "Command+S"
                if (this.viewChangeButton) {
                    this.viewChangeButton.triggerClick();
                }
                event.preventDefault();
            }
        });
    }

    async open() {
        this.fieldModels = (await fieldModelBuilder.getFieldModels(this.formId)).filter(f => !this.contentType.primaryKeys.includes(f.descriptor.camelCaseId));
        this.formBuilder = new FormBuilder(this.app, this);

        if (this.contentId) {
            this.setTitle(`Edit ${await nameProvider.getNameOf(this.content, this.contentType.id)}`);
        } else {
            this.setTitle(`New ${this.contentType.name}`);
        }

        if (this.contentId && this.contentType.isRoutable) {
            var urls = await urlFetcher.fetch(
                    `GetUrl/GetUrl?${this.contentId.map(key => `keyValues=${encodeURIComponent(JSON.stringify(key))}`).join('&')}&contentTypeId=${encodeURIComponent(this.contentType.id)}`,
                    {
                        credentials: 'include',
                        method: 'GET',
                        headers: { 'Content-Type': 'application/json' }
                    },
                    'Could not get URL'
                );

            const button = new Button('View').setInherit().onClick(() => menu.toggle());
            const menu = new PopupMenu(button.element);
                    
            if (urls.length) {
                urls.forEach(item =>
                    menu.addItem(listItem => {
                        listItem.setText(item);
                        listItem.onClick(() => {
                            window.open(`${location.origin}/${item}`, '_blank');
                        });
                    })
                );
            } else {
                menu.addItem(item => item.setDisabled().setText('(no items)'));
            }

            this.setToolbar(menu);
        }

        this.buildForm();

        const pendingChanges = this.app.changeTracker.getFor(this.contentId, this.contentType.id);
        this.saveButton = new Button('Save').setPrimary().setStyle({ marginLeft: '10px' }).setDisabled(!pendingChanges).onClick(() => {
            this.app.changeTracker.applyFor(this.contentId, this.contentType.id);
            this.onCompleteCallbacks.forEach(callback => callback());
        });
        this.viewChangeButton = new Button('View changes').setStyle({ marginLeft: 'auto' }).onClick(() => this.app.changeTracker.saveChange());
        this.app.changeTracker.setReferenceEvents(this.viewChangeButton, 'secondary', this.contentId, this.contentType.id);
        this.app.changeTracker.setReferenceEvents(this.saveButton, 'secondary', this.contentId, this.contentType.id);
        this.app.changeTracker.update();
        this.setFooter(this.viewChangeButton, this.saveButton);
    }

    async buildForm() {
        try {
            var pendingContent = this.app.changeTracker.mergeWithPendingChanges(this.contentId, this.contentType.id, this.content);
            var onChangeCallback = (name, value, originalValue) => this.app.changeTracker.save(this.contentId, this.contentType.id, {
                name,
                value,
                originalValue
            });

            var groups = [...new Set((await fieldDescriptorProvider.getFor(this.formId)).map(fieldDescriptor => fieldDescriptor.group))].sort();

            if (groups.length == 1) {
                var form = this.formBuilder.build(pendingContent, this.fieldModels.filter(fieldModel => fieldModel.descriptor.group == groups[0]), onChangeCallback)

                this.setContent(form);
            } else {
                var tabSystem = new TabSystem();

                if (groups.indexOf(null) != -1) {
                    tabSystem.addTab('General', async () => {
                        var element = document.createElement('div');
                        var form = this.formBuilder.build(pendingContent, this.fieldModels.filter(fieldModel => fieldModel.descriptor.group == null), onChangeCallback);
                        form.appendTo(element);
                        return element;
                    });
                }

                groups.filter(g => g != null).forEach(group => tabSystem.addTab(group, async () => {
                    var element = document.createElement('div');
                    var form = this.formBuilder.build(pendingContent, this.fieldModels.filter(fieldModel => fieldModel.descriptor.group == group), onChangeCallback);
                    form.appendTo(element);
                    return element;
                }));

                this.setContent(tabSystem);
            }
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not build form --- ${error.message}`));
            throw error;
        }
    }

    onComplete(callback) {
        this.onCompleteCallbacks.push(callback);

        return this;
    }
}

export default EditContentBlade;