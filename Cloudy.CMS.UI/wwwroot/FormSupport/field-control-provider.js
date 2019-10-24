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
            });

        this.typeModulesPromises = {};
    }
    
    getFor(field) {
        return this
            .modulePathsPromise
            .then(modulePaths => {
                var modulePath = modulePaths[field.control.id];

                if (!this.typeModulesPromises[field.control.id]) {
                    this.typeModulesPromises[field.control.id] = import(modulePath);
                }

                return this.typeModulesPromises[field.control.id];
            })
            .then(module => module.default);
    }
}

export default new FieldControlProvider();