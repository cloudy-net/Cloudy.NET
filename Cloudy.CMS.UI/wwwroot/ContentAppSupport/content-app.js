import App from '../app.js';
import ListContentTypesBlade from './list-content-types-blade.js';
import changeTracker from './utils/change-tracker.js';
import nav from '../nav.js';
import Button from '../components/button/button.js';
import PendingChangesBlade from './pending-changes-blade.js';



/* APP */

class ContentApp extends App {
    listContentTypesBlade = null;

    constructor() {
        super();
        this.listContentTypesBlade = new ListContentTypesBlade(this);

        const changeTrackerButton = new Button('No changes').setDisabled().onClick(() => this.addBladeAfter(new PendingChangesBlade(this), this.listContentTypesBlade));
        changeTracker.onUpdate(() => {
            const changesCount = changeTracker?.pendingChanges?.length;
            changeTrackerButton.setText(!changesCount ? 'No changes' : (changesCount > 1 ? `${changesCount} changes` : '1 change'));
            changeTrackerButton.setPrimary(changesCount > 0);
            changeTrackerButton.setDisabled(changesCount <= 0);
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