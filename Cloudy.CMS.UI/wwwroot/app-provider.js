import urlFetcher from './url-fetcher.js';

/* APP PROVIDER */

class AppProvider {
    constructor() {
        this.promise = this.init();
    }

    async init() {
        this.apps = await urlFetcher.fetch(`App/GetAll`, { credentials: 'include' }, 'Could not get apps');
        this.appsById = {};
        this.apps.forEach(app => this.appsById[app.id] = app);
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