import urlFetcher from '../util/url-fetcher.js';
import ContentNotFound from './content-not-found.js';

class ContentGetter {
    async get(entityReference) {
        const result = await urlFetcher.fetch(
            `/Admin/api/form/content/get`,
            {
                credentials: 'include',
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(entityReference)
            },
            `Could not get content ${JSON.stringify(entityReference.keyValues)} (${entityReference.entityTypeId})`,
            {
                410: () => new ContentNotFound(entityReference)
            }
        );

        return result.Value;
    }
}

export default new ContentGetter();