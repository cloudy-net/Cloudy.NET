import App from '../app.js';
import ListContentTypesBlade from './list-content-types-blade.js';
import changeTracker from './change-tracker.js';
import nav from '../nav.js';
import Button from '../button.js';
import PendingChangesBlade from './pending-changes-blade.js';



/* APP */

class ContentApp extends App {
    listContentTypesBlade = null;

    constructor() {
        super();
        this.listContentTypesBlade = new ListContentTypesBlade(this);

        const changeTrackerButton = new Button('No changes').setDisabled().onClick(() => this.addBladeAfter(new PendingChangesBlade(this), this.listContentTypesBlade));
        changeTracker.onUpdate(() => {
            let changeCount = 0;
            changeTracker.pendingChanges.forEach(c => changeCount += c.changedFields.length);
            changeTrackerButton.setText(changeCount == 0 ? 'No changes' : (changeCount > 1 ? `${changeCount} changes` : '1 change'));
            changeTrackerButton.setPrimary(changeCount > 0);
            changeTrackerButton.setDisabled(changeCount <= 0);
        });
        nav.setAction(changeTrackerButton);

        this.stateUpdate = async () => {
            for (var i = 0; i < this.blades.length; i++) {
                var blade = this.blades[i];

                if (!blade.stateUpdate) {
                    break;
                }

                await blade.stateUpdate();
            }
        };
    }

    open() {
        if (!this.blades.length) {
            this.addBlade(this.listContentTypesBlade);
        }

        window.addEventListener("hashchange", this.stateUpdate);
        this.stateUpdate();
    }

    close() {
       window.removeEventListener("hashchange", this.stateUpdate);
    }
};

export default ContentApp;