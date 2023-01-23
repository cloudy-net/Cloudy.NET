import urlFetcher from '../util/url-fetcher.js';
import ContentNotFound from './content-not-found.js';

class ContentGetter {
    async get(contentReference) {
        const result = await urlFetcher.fetch(
            `/Admin/api/form/content/get`,
            {
                credentials: 'include',
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(contentReference)
            },
            `Could not get content ${JSON.stringify(contentReference.keyValues)} (${contentReference.entityTypeId})`,
            {
                410: () => new ContentNotFound(contentReference)
            }
        );

        return result.Value;
    }
}

export default new ContentGetter();