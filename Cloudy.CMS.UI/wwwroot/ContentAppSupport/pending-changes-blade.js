import Blade from "../blade.js";
import Button from "../button.js";
import ListItem from "../ListSupport/list-item.js";
import List from "../ListSupport/list.js";
import nameProvider from "./utils/name-provider.js";
import contentTypeProvider from "./utils/content-type-provider.js";
import PendingChangesDiffBlade from "./pending-changes-diff-blade.js";
import contentGetter from './utils/content-getter.js';
import changeTracker from "./change-tracker.js";



/* PENDING CHANGES BLADE */

class PendingChangesBlade extends Blade {
    constructor(app) {
        super();
        this.app = app;
        
        this.setTitle('Pending changes');
        this.saveButton = new Button('Save all');
        this.setFooter(
            this.saveButton
                .setPrimary()
                .setStyle({ marginLeft: 'auto' })
                .onClick(async () => {
                    this.saveButton.setDisabled();
                    changeTracker.apply(null, () => {
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

        for (const change of changeTracker.pendingChanges) {
            changesByContentType[change.contentTypeId] = change;
        }

        var allContentTypes = await contentTypeProvider.getAll();

        var contentTypes = Object.keys(changesByContentType).map(contentTypeId => allContentTypes.find(contentType => contentType.id == contentTypeId)).sort(a => a.name, b => b.name);

        for (const contentType of contentTypes) {
            list.addSubHeader(contentType.pluralName);
        }

        for (let change of changeTracker.pendingChanges) {
            let name = '';
            if (change.contentId) {
                const content = await contentGetter.get(change.contentId, change.contentTypeId);
                name = await nameProvider.getNameOf(content, change.contentTypeId);
            } else {
                const content = changeTracker.mergeWithPendingChanges(null, change.contentTypeId, {});
                name = await nameProvider.getNameOf(content, change.contentTypeId);
            }
            const changedCount = change.changedFields.length;
            const subText = change.contentId ? `Changed fields: ${changedCount}` : `New ${(await contentTypeProvider.get(change.contentTypeId)).lowerCaseName}`;

            list.addItem(new ListItem().setText(name).setSubText(subText).onClick(() => {
                this.app.addBladeAfter(new PendingChangesDiffBlade(this.app, this, change), this);
            }));
        }

        this.setContent(list);
    }
}

export default PendingChangesBlade;