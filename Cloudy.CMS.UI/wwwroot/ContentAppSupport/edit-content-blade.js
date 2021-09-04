import Blade from '../blade.js';
import Button from '../button.js';
import LinkButton from '../link-button.js';
import ContextMenu from '../ContextMenuSupport/context-menu.js';
import TabSystem from '../TabSupport/tab-system.js';
import notificationManager from '../NotificationSupport/notification-manager.js';
import FormBuilder from '../FormSupport/form-builder.js';
import fieldDescriptorProvider from '../FormSupport/field-descriptor-provider.js';
import fieldModelBuilder from '../FormSupport/field-model-builder.js';
import primaryKeyProvider from './primary-key-provider.js';
import contentNameProvider from './content-name-provider.js';
import urlFetcher from '../url-fetcher.js';



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
                if (this.saveButton) {
                    this.saveButton.triggerClick();
                }
                event.preventDefault();
            }
        });
    }

    async open() {
        this.fieldModels = (await fieldModelBuilder.getFieldModels(this.formId)).filter(f => !this.contentType.primaryKeys.includes(f.descriptor.camelCaseId));

        this.formBuilder = new FormBuilder(this.app, this);

        if (this.contentId) {
            this.setTitle(`Edit ${await contentNameProvider.getNameOf(this.contentId, this.contentType.id)}`);
        } else {
            this.setTitle(`New ${this.contentType.name}`);
        }

        if (this.contentId && this.contentType.isRoutable) {
            var urls = await urlFetcher.fetch(
                    `GetUrl/GetUrl?id=${encodeURIComponent(JSON.stringify(this.contentId))}&contentTypeId=${encodeURIComponent(this.contentType.id)}`,
                    {
                        credentials: 'include',
                        method: 'GET',
                        headers: { 'Content-Type': 'application/json' }
                    },
                    'Could not get URL'
                );

            if (!urls.length) {
                return;
            }

            this.setToolbar(new LinkButton('View', `${location.origin}/${urls[0]}`, '_blank').setInherit());
        }

        this.buildForm();

        this.saveButton = new Button('Save').setPrimary().onClick(() => console.log('TODO: save edit content'));

        this.setFooter(this.saveButton)
    }

    async buildForm() {
        try {
            var groups = [...new Set((await fieldDescriptorProvider.getFor(this.formId)).map(fieldDescriptor => fieldDescriptor.group))].sort();

            if (groups.length == 1) {
                var form = this.formBuilder.build(
                    this.content,
                    this.fieldModels.filter(fieldModel => fieldModel.descriptor.group == groups[0]),
                    (path, value) => this.app.changeTracker.save(this.contentId, this.contentType.id, this.name, {
                        path,
                        value
                    })
                )
                
                this.setContent(form);
            } else {
                var tabSystem = new TabSystem();

                if (groups.indexOf(null) != -1) {
                    tabSystem.addTab('General', async () => {
                        var element = document.createElement('div');
                        var form = this.formBuilder.build(this.content, this.fieldModels.filter(fieldModel => fieldModel.descriptor.group == null));
                        form.appendTo(element);
                        return element;
                    });
                }

                groups.filter(g => g != null).forEach(group => tabSystem.addTab(group, async () => {
                    var element = document.createElement('div');
                    var form = this.formBuilder.build(this.content, this.fieldModels.filter(fieldModel => fieldModel.descriptor.group == group));
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