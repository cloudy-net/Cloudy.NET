import contentGetter from "./content-getter.js";
import contentTypeProvider from "./content-type-provider.js";



/* CONTENT NAME PROVIDER */

class ContentNameProvider {
    contentNames = {};

    /**
     * Gets (and caches) the name of a content object.
     * @param {string} contentId
     * @param {string} contentTypeId
     */
    async getNameOf(contentId, contentTypeId) {
        if (!contentId) {
            throw new Error('No content id was supplied');
        }
        if (!contentTypeId) {
            throw new Error('No content type id was supplied');
        }

        if (!this.contentNames[contentTypeId]) {
            this.contentNames[contentTypeId] = {};
        }

        var cacheKey = contentId instanceof Array ? JSON.stringify(contentId) : contentId;

        if (this.contentNames[contentTypeId][cacheKey]) {
            return this.contentNames[contentTypeId][cacheKey];
        }

        var content = await contentGetter.get(contentId, contentTypeId);
        var contentType = await contentTypeProvider.get(contentTypeId);

        var name = this.generateNameOf(content, contentId, contentType);

        this.contentNames[contentTypeId][cacheKey] = name;

        return name;
    }

    /**
     * Generates the name of a content object.
     * @param {object} content
     * @param {object} contentType
     */
    generateNameOf(content, contentId, contentType) {
        var name = '';

        if (!contentType.isSingleton) {
            if (contentType.isNameable) {
                name = contentType.nameablePropertyName ? content[contentType.nameablePropertyName] : content.name;
            }
        }
        if (!name) {
            if (contentId) {
                name = `${contentType.name} ${(contentId instanceof Array ? JSON.stringify(contentId) : contentId)}`;
            } else {
                name = contentType.name;
            }
        }

        return name;
    }

    getContentIdFormatted(contentId, contentTypeId) {
        return `${contentId}|${contentTypeId}`;
    }
}

export default new ContentNameProvider();