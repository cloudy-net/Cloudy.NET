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

        if (this.apps[appDescriptor.id]) {
            this.element.appendChild(app.element);
            return;
        }

        import(`../../${appDescriptor.modulePath}`)
            .then(module => {
                var app = new module.default();

                this.element.insertBefore(app.element, this.element.firstChild);

                app.open();
            });
    }
}

export default Portal;