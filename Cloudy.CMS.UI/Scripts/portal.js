import Nav from "./nav.js";



/* PORTAL */

class Portal {
    constructor() {
        this.apps = {};
        this.element = document.createElement('poetry-ui-portal');

        this.nav = new Nav(this);

        if (document.readyState != 'loading') {
            document.body.appendChild(this.element);
        } else {
            document.addEventListener('DOMContentLoaded', document.body.appendChild(this.element));
        }
    }

    openApp(appDescriptor) {
        [...this.element.querySelectorAll('poetry-ui-app')].forEach(a => this.element.removeChild(a));

        this.nav.openApp(appDescriptor);

        history.pushState(null, null, `#${appDescriptor.id}`);

        var open = app => {
            this.element.insertBefore(app.element, this.element.firstChild);
            app.openStartBlade();
        }

        if (this.apps[appDescriptor.id]) {
            open(this.apps[appDescriptor.id]);
            return;
        }

        import(`../../${appDescriptor.modulePath}`).then(module => open(new module.default()));
    }
}

export default Portal;