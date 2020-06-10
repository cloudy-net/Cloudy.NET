import Nav from "./nav.js";



/* PORTAL */

class Portal {
    currentAppId = null;

    constructor() {
        this.apps = {};
        this.element = document.createElement('cloudy-ui-portal');

        this.nav = new Nav(this);

        if (document.readyState != 'loading') {
            document.body.appendChild(this.element);
        } else {
            document.addEventListener('DOMContentLoaded', document.body.appendChild(this.element));
        }
    }

    setTitle(value) {
        this.nav.setTitle(value);
    }

    openApp(appDescriptor) {
        if (appDescriptor.id == this.currentAppId) {
            return;
        }

        this.nav.openApp(appDescriptor);

        history.pushState(null, null, `#${appDescriptor.id}`);

        var open = app => {
            this.element.appendChild(app.element);
            app.openStartBlade();
        }

        if (this.apps[appDescriptor.id]) { // app is already open
            open(this.apps[appDescriptor.id]);
            return;
        }

        if (this.apps[this.currentAppId]) {
            this.apps[this.currentAppId].element.remove();
        }
        this.currentAppId = appDescriptor.id;

        import(appDescriptor.modulePath.indexOf('/') == 0 ? appDescriptor.modulePath : `./${appDescriptor.modulePath}`).then(module => {
            if (appDescriptor.id != this.currentAppId) {
                return; // switched app during loading. abort.
            }

            open(new module.default());
        });
    }
}

export default Portal;