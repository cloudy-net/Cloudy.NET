import notificationManager from "../NotificationSupport/notification-manager.js";

class FieldControlProvider {
    constructor() {
        this.modulePathsPromise = fetch('Control/ModulePaths', {
            credentials: 'include'
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`${response.status} (${response.statusText})`);
                }

                return response.json();
            })
            .catch(error => notificationManager.addNotification(item => item.setText(`Could not get module paths for form field controls (${error.name}: ${error.message})`)));;

        this.typeModulesPromises = {};
    }
    
    async getFor(field) {
        var modulePaths = await this.modulePathsPromise;
        var modulePath = modulePaths[field.control.id];

        if (!this.typeModulesPromises[field.control.id]) {
            this.typeModulesPromises[field.control.id] = import(modulePath.indexOf('/') == 0 ? modulePath : `../${modulePath}`);
        }

        var module;

        try {
            module = await this.typeModulesPromises[field.control.id];
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not load field control ${field.control.id} (${error.name}: ${error.message})`));
            throw error;
        }

        return module.default;
    }
}

export default new FieldControlProvider();