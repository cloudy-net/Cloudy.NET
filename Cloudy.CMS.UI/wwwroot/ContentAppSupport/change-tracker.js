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
    changedFieldsModel = [];
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

        this.buildChangedModel();
    }

    reset(name) {
        const index = this.pendingChanges.findIndex(item => item.name === name);
        if (index !== -1) {
            this.pendingChanges.splice(index, 1);
            this.update();
        }
    }

    resetAll() {
        this.pendingChanges = [];
        this.changedFieldsModel = [];
        this.update();
    }

    async apply(callback) {
        await contentSaver.save(this.changedFieldsModel);
        state.set(this.changedFieldsModel[0].contentTypeId);
        this.resetAll();
        callback && callback();
    }

    buildControlName(contentTypeId, contentId, fieldId) {
        return `${contentTypeId}__${contentId}__${fieldId}`;
    }

    buildChangedModel() {
        this.pendingChanges.forEach(item => {
            //add group by contentTypeId contentTypeId and contentId
            if (!this.changedFieldsModel.some(e => e.contentTypeId === item.contentTypeId && e.contentId === item.contentId)) {
                this.changedFieldsModel.push({
                    contentTypeId: item.contentTypeId,
                    contentId: item.contentId,
                    keyValues: [item.contentId]
                });
            }
            // add changed fields group by contentTypeId and contentId
            const index = this.changedFieldsModel.findIndex(e => e.contentTypeId === item.contentTypeId && e.contentId === item.contentId);
            if (typeof this.changedFieldsModel[index].changedFields === "undefined") {
                this.changedFieldsModel[index].changedFields = [];
            }
            // Add or update path & value for changed fields
            if (!this.changedFieldsModel[index].changedFields.some(c => c.path === item.contentAsJson.path)) {
                this.changedFieldsModel[index].changedFields.push({
                    path: item.contentAsJson.path,
                    value: item.contentAsJson.value
                });
            } else {
                const pathIndex = this.changedFieldsModel[index].changedFields.findIndex(c => c.path === item.contentAsJson.path);
                this.changedFieldsModel[index].changedFields[pathIndex].value = item.contentAsJson.value;
            }
        });
        return this.changedFieldsModel;
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
        this.setFooter(this.saveButton.setPrimary().setStyle({ marginLeft: 'auto' }).onClick(() => this.changeTracker.apply(() => {
            this.saveButton.setDisabled();
            var list = new List();
            list.addItem(new ListItem().setText('Content has been saved.'))
            this.setContent(list);
        })));
    }

    async open() {
        var list = new List();

        for (let change of this.changeTracker.changedFieldsModel) {
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