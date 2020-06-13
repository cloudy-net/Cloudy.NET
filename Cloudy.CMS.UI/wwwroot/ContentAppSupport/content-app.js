import App from '../app.js';
import ListContentTypesBlade from './list-content-types-blade.js';
import state from '../state.js';



/* APP */

class ContentApp extends App {
    listContentTypesBlade = null;

    constructor() {
        super();
        this.listContentTypesBlade = new ListContentTypesBlade(this);

        this.stateUpdate = async () => {
            state.update();
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