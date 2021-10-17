import Blade from "../blade.js";
import Button from "../button.js";
import ListItem from "../ListSupport/list-item.js";
import List from "../ListSupport/list.js";
import nameProvider from "./name-provider.js";
import contentTypeProvider from "./content-type-provider.js";
import PendingChangesDiffBlade from "./pending-changes-diff-blade.js";



/* PENDING CHANGES BLADE */

class PendingChangesBlade extends Blade {
    changeTracker;
    constructor(app, changeTracker) {
        super();
        this.app = app;
        this.changeTracker = changeTracker;

        this.setTitle('Pending changes');
        this.saveButton = new Button('Save all');
        this.setFooter(
            this.saveButton
                .setPrimary()
                .setStyle({ marginLeft: 'auto' })
                .onClick(async () => {
                    this.saveButton.setDisabled();
                    this.changeTracker.apply(null, () => {
                        this.setContent();
                        this.app.removeBladesAfter(this);
                    });
                    this.saveButton.setDisabled(false);
                })
        );
    }

    async open() {
        var list = new List();

        var changesByContentType = {};

        for (const change of this.changeTracker.pendingChanges) {
            changesByContentType[change.contentTypeId] = change;
        }

        var allContentTypes = await contentTypeProvider.getAll();

        var contentTypes = Object.keys(changesByContentType).map(contentTypeId => allContentTypes.find(contentType => contentType.id == contentTypeId)).sort(a => a.name, b => b.name);

        for (const contentType of contentTypes) {
            list.addSubHeader(contentType.pluralName);
        }

        for (let change of this.changeTracker.pendingChanges) {
            let name = '';
            if (change.contentId) {
                name = await nameProvider.getNameOf(change.contentId, change.contentTypeId);
            }
            const changedCount = change.changedFields.length;
            const subText = changedCount > 1 ? `Changes: ${changedCount}` : `Change: ${changedCount}`;

            list.addItem(new ListItem().setText(name).setSubText(subText).onClick(() => {
                this.app.addBladeAfter(new PendingChangesDiffBlade(this.app, this.changeTracker, this, change), this);
            }));
        }

        this.setContent(list);
    }
}

export default PendingChangesBlade;