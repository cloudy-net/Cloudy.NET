import urlFetcher from '../../url-fetcher.js';
import ContentNotFound from './content-not-found.js';

/* CONTENT GETTER */

class ContentGetter {
    contentByContentTypeAndId = {};

    /**
     * Gets content of the specified type.
     * @param {Array} contentId
     * @param {string} contentTypeId
     */
    async get(contentId, contentTypeId) {
        if (!contentId) {
            throw new Error('No content id was supplied');
        }
        if (!contentTypeId) {
            throw new Error('No content type id was supplied');
        }

        const contentIdCacheKey = JSON.stringify(contentId);

        if (!this.contentByContentTypeAndId[contentTypeId]) {
            this.contentByContentTypeAndId[contentTypeId] = {};
        }

        if (this.contentByContentTypeAndId[contentTypeId][contentIdCacheKey]) {
            return this.contentByContentTypeAndId[contentTypeId][contentIdCacheKey];
        }

        var content = await urlFetcher.fetch(
            `ContentGetter/Get`,
            {
                credentials: 'include',
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    keyValues: contentId,
                    contentTypeId: contentTypeId
                })
            },
            `Could not get content ${contentId} (${contentTypeId})`,
            {
                410: () => new ContentNotFound(contentId, contentTypeId)
            }
        );
        content = content.Value;
        this.contentByContentTypeAndId[contentTypeId][contentIdCacheKey] = content;
        return content;
    }

    clearCacheFor(contentId, contentTypeId) {
        const contentIdCacheKey = JSON.stringify(contentId);

        if (!this.contentByContentTypeAndId[contentTypeId]) {
            this.contentByContentTypeAndId[contentTypeId] = {};
        }

        delete this.contentByContentTypeAndId[contentTypeId][contentIdCacheKey];
    }
}

export default new ContentGetter();