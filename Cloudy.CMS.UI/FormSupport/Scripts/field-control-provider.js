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
                var modulePath = modulePaths[field.Control.Id];

                if (!this.typeModulesPromises[field.Control.Id]) {
                    this.typeModulesPromises[field.Control.Id] = import(modulePath);
                }

                return this.typeModulesPromises[field.Control.Id];
            })
            .then(module => module.default);
    }
}

export default new FieldControlProvider();