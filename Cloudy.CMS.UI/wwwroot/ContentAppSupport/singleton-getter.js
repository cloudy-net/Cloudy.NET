import urlFetcher from "../url-fetcher.js";

class SingletonGetter {
    async get(contentTypeId) {
        return await urlFetcher.fetch(`Singleton/Get?id=${contentTypeId}`, { credentials: 'include' }, 'Could not get singleton');
    }
}

export default new SingletonGetter();