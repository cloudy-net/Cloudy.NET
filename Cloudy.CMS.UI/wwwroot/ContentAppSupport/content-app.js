import App from '../app.js';
import ListContentTypesBlade from './list-content-types-blade.js';



/* APP */

class ContentApp extends App {
    constructor() {
        super();
        this.open(new ListContentTypesBlade(this));
    }
};

export default ContentApp;