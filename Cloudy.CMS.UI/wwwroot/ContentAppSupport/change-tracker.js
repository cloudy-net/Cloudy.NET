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

    save(contentIdFormatted, contentAsJson) {
        const index = this.pendingChanges.findIndex(c => c.type === 'save' && c.contentIdFormatted === contentIdFormatted);

        if (index !== -1) {
            const {path, value, originalValue} = contentAsJson;
            const changeFieldIndex = this.pendingChanges[index].changedFields.findIndex(f => f.path === path);
            if (changeFieldIndex === -1) {
                this.pendingChanges[index].changedFields.push(contentAsJson);
            } else {
                if (value === originalValue) {
                    this.pendingChanges[index].changedFields.splice(changeFieldIndex, 1);
                } else {
                    this.pendingChanges[index].changedFields[changeFieldIndex].value = value;
                }
            }
        } else {
            this.pendingChanges.push({
                type: 'save',
                contentIdFormatted,
                changedFields: [{...contentAsJson}]
            });
        }
        
        this.update();
    }

    update() {
        let changeCount = 0;
        this.pendingChanges.forEach(c => {
            changeCount = changeCount + c.changedFields.length;
        });
        const changeText = changeCount <= 0 ? 'No changes': (changeCount > 1 ? `${changeCount} changes` : '1 change');
        this.button.setText(changeText);

        this.button.setDisabled(changeCount <= 0);
        this.button.setPrimary(changeCount > 0);
    }

    reset(name) {
        console.log('Resetting');
        const index = this.pendingChanges.findIndex(item => item.name === name);
        if (index !== -1) {
            this.pendingChanges.splice(index, 1);
            this.update();
        }
    }

    resetAll(callback) {
        this.pendingChanges = [];
        this.update();
        callback && callback();
    }

    async apply() {
        const contentToSave = this.pendingChanges.map(c => {
            const contentIdFormattedToArray = c.contentIdFormatted.split('|');
            const changedArray = c.changedFields.map(f => {
                const { originalValue, ...changedObj } = f;
                return changedObj;
            });
            return {
                keyValues: [contentIdFormattedToArray[0]],
                contentTypeId: contentIdFormattedToArray[1],
                changedFields: changedArray
            }
        })
        if (await contentSaver.save(contentToSave) == false) {
            return false; // fail
        }
        this.resetAll(() => window.location.reload());
    }

    getCurrentValueOfPath(contentIdFormatted, path) {
        const currentContentIndex = this.pendingChanges.findIndex(c => c.contentIdFormatted === contentIdFormatted);
        if (currentContentIndex !== -1) {
            const currentFieldIndex = this.pendingChanges[currentContentIndex].changedFields.findIndex(f => f.path === path);
            return currentFieldIndex !== -1 ? this.pendingChanges[currentContentIndex].changedFields[currentFieldIndex].value : '';
        }
        return '';
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
            const contentIdFormattedToArray = change.contentIdFormatted.split('|');
            const name = await contentNameProvider.getNameOf(contentIdFormattedToArray[0], contentIdFormattedToArray[1]);
            const changedCount = change.changedFields.length;
            const subText = changedCount > 1 ? `Changes: ${changedCount}` : `Change: ${changedCount}`;
         
            list.addSubHeader(name).addItem(new ListItem().setSubText(subText).onClick(() => console.log(change)));
        }

        this.setContent(list);
    }
}

export default ChangeTracker;