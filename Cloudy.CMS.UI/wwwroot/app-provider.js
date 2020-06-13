import NotificationManager from './NotificationSupport/notification-manager.js';



/* APP PROVIDER */

class AppProvider {
    constructor() {
        this.promise = this.init();
    }

    async init() {
        try {
            var response = await fetch(`App/GetAll`, { credentials: 'include' });

            if (!response.ok) {
                var text = await response.text();

                if (text) {
                    throw new Error(text.split('\n')[0]);
                } else {
                    text = response.statusText;
                }

                throw new Error(`${response.status} (${text})`);
            }

            this.apps = await response.json();
            this.appsById = {};
            this.apps.forEach(app => this.appsById[app.id] = app);
        } catch (error) {
            NotificationManager.addNotification(item => item.setText(`Could not get apps (${error.message})`));
            throw error;
        }
    }

    async getAll() {
        await this.promise;
        return this.apps;
    }

    async get(id) {
        await this.promise;

        if (!this.appsById[id]) {
            throw new Error(`App not found`);
        }

        return this.appsById[id];
    }
}

export default new AppProvider();