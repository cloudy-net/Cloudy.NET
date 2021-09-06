import App from '../app.js';
import ListContentTypesBlade from './list-content-types-blade.js';
import ChangeTracker from './change-tracker.js';
import nav from '../nav.js';



/* APP */

class ContentApp extends App {
    listContentTypesBlade = null;
    changeTracker = null;

    constructor() {
        super();
        this.listContentTypesBlade = new ListContentTypesBlade(this);
        this.changeTracker = new ChangeTracker(this, this.listContentTypesBlade);
        nav.setAction(this.changeTracker);

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