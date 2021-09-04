import Blade from "../blade.js";
import Button from "../button.js";
import ListItem from "../ListSupport/list-item.js";
import List from "../ListSupport/list.js";
import notificationManager from "../NotificationSupport/notification-manager.js";
import contentNameProvider from "./content-name-provider.js";
import contentSaver from "./content-saver.js";
import state from '../state.js';


/* CHANGE TRACKER */

class ChangeTracker {
    element = document.createElement('cloudy-ui-change-tracker');
    pendingChanges = [];
    changeExecutors = {
        save: contentSaver
    };

    constructor(app, parentBlade) {
        this.button = new Button('No changes').setDisabled().appendTo(this.element);

        this.button.onClick(() => {
            app.addBladeAfter(new PendingChangesBlade(app, this), parentBlade);
        });
       
        this.update();
    }

    save(contentId, contentTypeId, name, contentAsJson) {
        var index = this.pendingChanges.findIndex(c =>
            c.type == 'save' &&
            c.contentId == contentId &&
            c.contentTypeId == contentTypeId
            && c.name == name
        );

        if (index != -1) {
            this.pendingChanges.splice(index, 1);
        }

        this.pendingChanges.push({
            type: 'save',
            contentId,
            contentTypeId,
            name,
            contentAsJson
        });
        this.update();
    }

    update() {
        switch (this.pendingChanges.length) {
            case 0: this.button.setText('No changes'); break;
            case 1: this.button.setText('1 change'); break;
            default: this.button.setText(`${this.pendingChanges.length} changes`); break;
        }

        this.button.setDisabled(this.pendingChanges.length == 0);
        this.button.setPrimary(this.pendingChanges.length > 0);
    }

    reset(name) {
        console.log('Resetting');
        const index = this.pendingChanges.findIndex(item => item.name === name);
        if (index !== -1) {
            this.pendingChanges.splice(index, 1);
            this.update();
        }
    }

    resetAll() {
        this.pendingChanges = [];
        this.update();
    }

    async apply() {
        if (await contentSaver.save(this.pendingChanges) == false) {
            return false; // fail
        }
        state.set(this.changedFieldsModel[0].contentTypeId);
        this.resetAll();
    }

    buildControlName(contentTypeId, contentId, fieldId) {
        return `${contentTypeId}__${contentId}__${fieldId}`;
    }
}



/* PENDING CHANGES BLADE */

class PendingChangesBlade extends Blade {
    changeTracker;
    constructor(app, changeTracker) {
        super();
        this.app = app;
        this.changeTracker = changeTracker;

        this.setTitle('Pending changes');
        this.saveButton = new Button('Save');
        this.setFooter(
            this.saveButton
                .setPrimary()
                .setStyle({ marginLeft: 'auto' })
                .onClick(async () => {
                    this.saveButton.setDisabled();
                    if (await this.changeTracker.apply()) {
                        var list = new List();
                        list.addItem(new ListItem().setText('Content has been saved.'))
                        this.setContent(list);
                    }
                    this.saveButton.setDisabled(false);
                })
        );
    }

    async open() {
        var list = new List();

        for (let change of this.changeTracker.pendingChanges) {
            const name = await contentNameProvider.getNameOf(change.contentId, change.contentTypeId);
            const contentName = `${name} <br/> Content id: ${change.contentId}`;
            let contentText = '';
            change.changedFields.forEach(item => {
                contentText += `${item.path}: ${item.value} <br/>`; 
            });
            list.addItem(new ListItem().setText(contentName).setSubText(contentText).onClick(() => console.log(change)));
        }

        this.setContent(list);
    }
}

export default ChangeTracker;