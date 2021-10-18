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
        const url = `ContentGetter/Get?contentId=${contentId}&contentTypeId=${contentTypeId}`;
        var content = await urlFetcher.fetch(url, { credentials: 'include' }, `Could not get content ${contentId} (${contentTypeId})`);
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