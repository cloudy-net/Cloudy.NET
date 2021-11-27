import urlFetcher from '../../url-fetcher.js';

/* CONTENT GETTER */

class ContentGetter {
    contentByContentTypeAndId = {};

    /**
     * Gets content of the specified type.
     * @param {Array} contentId
     * @param {string} contentTypeId
     */
    async get(contentId, contentTypeId) {
        if (!this.contentByContentTypeAndId[contentTypeId]) {
            this.contentByContentTypeAndId[contentTypeId] = {};
        }

        if (this.contentByContentTypeAndId[contentTypeId][contentId]) {
            return this.contentByContentTypeAndId[contentTypeId][contentId];
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
            `Could not get content ${contentId} (${contentTypeId})`
        );
        content = content.Value;
        this.contentByContentTypeAndId[contentTypeId][contentId] = content;
        return content;
    }

    /**
     * Updates the cache of a specified content.
     * @param {object} content
     */
    set(content) {
        if (!this.contentByContentTypeAndId[content.contentTypeId]) {
            this.contentByContentTypeAndId[content.contentTypeId] = {};
        }

        this.contentByContentTypeAndId[contentTypeId][contentId] = content.content;
    }
}

export default new ContentGetter();