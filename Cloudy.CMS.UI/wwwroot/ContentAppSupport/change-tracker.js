import Blade from "../blade.js";
import Button from "../button.js";
import ListItem from "../ListSupport/list-item.js";
import List from "../ListSupport/list.js";
import notificationManager from "../NotificationSupport/notification-manager.js";
import contentNameProvider from "./content-name-provider.js";
import contentSaver from "./content-saver.js";



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

    save(contentId, contentTypeId, contentAsJson) {
        var index = this.pendingChanges.findIndex(c =>
            c.type == 'save' &&
            c.contentId == contentId &&
            c.contentTypeId == contentTypeId
        );

        if (index != -1) {
            this.pendingChanges.splice(index, 1);
        }

        this.pendingChanges.push({
            type: 'save',
            contentId,
            contentTypeId,
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

    async apply() {
        for (var change of this.changeTracker.pendingChanges) {
            var result;
            switch (change.type) {
                case 'save': result = await contentSaver.save(change); break;
                default: notificationManager.addNotification(new Notification(`Unknown change type ${change.type}`)); break;
            }
            if (result.success) {
                continue;
            }
        }
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

        this.setFooter(new Button('Save').setPrimary().setStyle({ marginLeft: 'auto' }).onClick(this.changeTracker.apply.bind(this)));
    }

    async open() {
        var list = new List();

        for (var change of this.changeTracker.pendingChanges) {
            var name = await contentNameProvider.getNameOf(change.contentId, change.contentTypeId);
            list.addItem(new ListItem().setText(name).setSubText(change.type).onClick(() => console.log(change)));
        }

        this.setContent(list);
    }
}

export default ChangeTracker;