import Blade from '../blade.js';
import Button from '../button.js';
import TabSystem from '../TabSupport/tab-system.js';
import FormBuilder from '../FormSupport/form-builder.js';
import fieldDescriptorProvider from '../FormSupport/field-descriptor-provider.js';
import fieldModelBuilder from '../FormSupport/field-model-builder.js';
import primaryKeyProvider from './utils/primary-key-provider.js';
import nameProvider from './utils/name-provider.js';
import urlFetcher from '../url-fetcher.js';
import PopupMenu from '../PopupMenuSupport/popup-menu.js';
import changeTracker from './change-tracker.js';
import PendingChangesDiffBlade from './pending-changes-diff-blade.js';



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
        if (this.contentId) {
            this.setTitle(`Edit ${await nameProvider.getNameOf(this.content, this.contentType.id)}`);
        } else {
            this.setTitle(`New ${this.contentType.name}`);
        }

        if (this.contentId && this.contentType.isRoutable) {
            var urls = await urlFetcher.fetch(
                    `GetUrl/GetUrl`,
                    {
                        credentials: 'include',
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({
                            keyValues: this.contentId,
                            contentTypeId: this.contentType.id
                        })
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

        this.saveButton = new Button('Save now').setPrimary().setStyle({ marginLeft: '10px' }).onClick(() => {
            changeTracker.applyFor(this.contentId, this.contentType.id);
            this.onCompleteCallbacks.forEach(callback => callback());
        });

        this.viewChangeButton = new Button('Review changes').setStyle({ marginLeft: 'auto' }).onClick(() => this.app.addBladeAfter(new PendingChangesDiffBlade(this.app, changeTracker.getFor(this.contentId, this.contentType.id)).onUndo(() => this.buildForm()), this));
        this.pendingChangesUpdateCallback = () => {
            const pendingChanges = changeTracker.getFor(this.contentId, this.contentType.id);

            this.viewChangeButton.setDisabled(pendingChanges && !pendingChanges.changedFields.length);
            this.saveButton.setDisabled(pendingChanges && !pendingChanges.changedFields.length);
        };
        changeTracker.onUpdate(this.pendingChangesUpdateCallback);
        this.onClose(() => changeTracker.removeOnUpdate(this.pendingChangesUpdateCallback));
        this.pendingChangesUpdateCallback();
        this.setFooter(this.viewChangeButton, this.saveButton);
    }

    async buildForm() {
        changeTracker.setReferenceObject({ ...this.content }, this.contentId, this.contentType.id);
        const fieldModels = (await fieldModelBuilder.getFieldModels(this.formId)).filter(f => !this.contentType.primaryKeys.includes(f.descriptor.camelCaseId));
        const formBuilder = new FormBuilder(this.app, this);
        
        var pendingContent = changeTracker.mergeWithPendingChanges(this.contentId, this.contentType.id, this.content);
        var onChangeCallback = (name, change) => changeTracker.addChange(this.contentId, this.contentType.id, name, change);

        var groups = [...new Set((await fieldDescriptorProvider.getFor(this.formId)).map(fieldDescriptor => fieldDescriptor.group))].sort();

        if (groups.length == 1) {
            var form = formBuilder.build(pendingContent, fieldModels.filter(fieldModel => fieldModel.descriptor.group == groups[0])).onChange(onChangeCallback)

            this.setContent(form);
        } else {
            var tabSystem = new TabSystem();

            if (groups.indexOf(null) != -1) {
                tabSystem.addTab('General', async () => {
                    var element = document.createElement('div');
                    var form = formBuilder.build(pendingContent, fieldModels.filter(fieldModel => fieldModel.descriptor.group == null)).onChange(onChangeCallback);
                    form.appendTo(element);
                    return element;
                });
            }

            groups.filter(g => g != null).forEach(group => tabSystem.addTab(group, async () => {
                var element = document.createElement('div');
                var form = formBuilder.build(pendingContent, fieldModels.filter(fieldModel => fieldModel.descriptor.group == group)).onChange(onChangeCallback);
                form.appendTo(element);
                return element;
            }));

            this.setContent(tabSystem);
        }
    }

    onComplete(callback) {
        this.onCompleteCallbacks.push(callback);

        return this;
    }
}

export default EditContentBlade;