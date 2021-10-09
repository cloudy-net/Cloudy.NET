import Blade from "../blade.js";
import Button from "../button.js";



/* PENDING CHANGES DIFF BLADE */

class PendingChangesDiffBlade extends Blade {
    changeTracker;
    constructor(app, change) {
        super();
        this.app = app;
        this.change = change;

        this.setTitle(`Pending changes (${change.changedFields.length})`);
        this.saveButton = new Button('Undo changes');
        this.setFooter(
            this.saveButton
                .setStyle({ marginLeft: 'auto' })
                .onClick(async () => {
                    confirm('Undo changes? This is not reversible');
                })
        );
    }

    async open() {
        this.setContent();
    }
}

export default PendingChangesDiffBlade;