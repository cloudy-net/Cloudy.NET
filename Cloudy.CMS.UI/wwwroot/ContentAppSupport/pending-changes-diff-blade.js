import Blade from "../blade.js";
import Button from "../button.js";
import ListItem from "../ListSupport/list-item.js";
import List from "../ListSupport/list.js";
import contentNameProvider from "./content-name-provider.js";
import contentTypeProvider from "./content-type-provider.js";



/* PENDING CHANGES DIFF BLADE */

class PendingChangesDiffBlade extends Blade {
    changeTracker;
    constructor(app, change) {
        super();
        this.app = app;
        this.changeTracker = changeTracker;

        this.setTitle('Pending changes');
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