import urlFetcher from "../../util/url-fetcher.js";

class SingletonGetter {
    async get(contentTypeId) {
        let content = await urlFetcher.fetch(
            `Singleton/Get?id=${contentTypeId}`,
            { credentials: 'include' },
            'Could not get singleton'
        );

        content = content.Value;

        return content;
    }
}

export default new SingletonGetter();