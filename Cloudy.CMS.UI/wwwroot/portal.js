import Nav from "./nav.js";
import notificationManager from "./NotificationSupport/notification-manager.js";



/* PORTAL */

class Portal {
    appDescriptors = [];
    apps = {};
    currentAppId = null;

    constructor(title, appDescriptors) {
        this.appDescriptors = appDescriptors;
        this.element = document.createElement('cloudy-ui-portal');
        this.nav = new Nav(this, title, appDescriptors);
        window.addEventListener("hashchange", () => { this.stateUpdate(); });
        this.stateUpdate();
    }

    stateUpdate() {
        var appId = location.hash.substr(1).split('/')[0];

        if (!appId && this.appDescriptors.length == 1) {
            history.replaceState(null, null, `#${this.appDescriptors[0].id}`);
            this.nav.update();
            this.stateUpdate();
            return;
        }

        if (appId == this.currentAppId) {
            return;
        }

        this.openApp(appId);
    }

    async openApp(id) {
        if (id == this.currentAppId) {
            return;
        }

        if (this.apps[this.currentAppId]) {
            this.apps[this.currentAppId].element.remove();
            this.apps[this.currentAppId].close();
        }

        this.currentAppId = id;

        var appDescriptor = this.appDescriptors.find(appDescriptor => appDescriptor.id == id);

        if (!appDescriptor) {
            notificationManager.addNotification(item => item.setText(`No such app: \`${id}\``));
            return;
        }

        var open = async app => {
            this.element.appendChild(app.element);
            await app.open();
        }

        if (this.apps[id]) {
            await open(this.apps[id]);
            return;
        }

        var module = await import(appDescriptor.modulePath.indexOf('/') == 0 ? appDescriptor.modulePath : `./${appDescriptor.modulePath}`);

        let app = new module.default();
        this.apps[id] = app;
        await open(app);
    }

    appendTo(element) {
        element.append(this.element);
    }
}

export default Portal;