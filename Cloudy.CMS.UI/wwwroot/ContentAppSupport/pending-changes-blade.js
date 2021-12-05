import Blade from "../blade.js";
import Button from "../button.js";
import ListItem from "../ListSupport/list-item.js";
import List from "../ListSupport/list.js";
import nameGetter from "./utils/name-getter.js";
import contentTypeProvider from "./utils/content-type-provider.js";
import PendingChangesDiffBlade from "./pending-changes-diff-blade.js";
import contentGetter from './utils/content-getter.js';
import changeTracker from "./utils/change-tracker.js";
import ContentNotFound from "./utils/content-not-found.js";



/* PENDING CHANGES BLADE */

class PendingChangesBlade extends Blade {
    constructor(app) {
        super();
        this.app = app;
        
        this.setTitle('Pending changes');

        this.onClose(() => changeTracker.removeOnUpdate(this.pendingChangesUpdateCallback));
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
            let contentType = await contentTypeProvider.get(change.contentTypeId);
            let content;
            if (change.contentId) {
                try{
                    content = await contentGetter.get(change.contentId, change.contentTypeId);
                }
                catch(exception){
                    if (exception instanceof ContentNotFound) {
                        list.addItem(new ListItem().setText(`Deleted ${contentType.lowerCaseName}`).setSubText(`${contentType.name} has been deleted`));
                        continue;
                    }
                }
            } else {
                content = changeTracker.mergeWithPendingChanges(null, change.contentTypeId, {});
            }
            const name = nameGetter.getNameOf(content, contentType);
            const changedCount = change.changedFields.length;
            const subText = change.remove ? 'Slated for removal' : change.contentId ? `Changed fields: ${changedCount}` : `New ${contentType.lowerCaseName}`;

            list.addItem(new ListItem().setText(name).setSubText(subText).onClick(async () => {
                const contentType = await contentTypeProvider.get(change.contentTypeId);
                this.app.addBladeAfter(new PendingChangesDiffBlade(this.app, change, contentType), this);
            }));
        }

        if (changeTracker.pendingChanges.length == 0) {
            list.addItem(new ListItem().setDisabled().setText('No more pending changes'));
        }

        this.setContent(list);

        const saveButton = new Button('Save all');
        this.setFooter(
            saveButton
                .setPrimary()
                .setDisabled(changeTracker.pendingChanges.length == 0)
                .setStyle({ marginLeft: 'auto' })
                .onClick(async () => {
                    saveButton.setDisabled();
                    changeTracker.apply(null, () => {
                        this.setContent();
                        this.app.removeBladesAfter(this);
                    });
                    saveButton.setDisabled(false);
                })
        );
    }
}

export default PendingChangesBlade;