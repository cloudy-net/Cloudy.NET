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
        if (!this.contentNames[contentTypeId]) {
            this.contentNames[contentTypeId] = {};
        }

        if (this.contentNames[contentTypeId][contentId]) {
            return this.contentNames[contentTypeId][contentId];
        }

        var content = await contentGetter.get(contentId, contentTypeId);
        var contentType = await contentTypeProvider.get(contentTypeId);

        var name = this.generateNameOf(content, contentType);

        this.contentNames[contentTypeId][contentId] = name;

        return name;
    }

    /**
     * Generates the name of a content object.
     * @param {object} content
     * @param {object} contentType
     */
    generateNameOf(content, contentType) {
        var name = '';

        if (!contentType.isSingleton) {
            if (contentType.isNameable) {
                name = contentType.nameablePropertyName ? content[contentType.nameablePropertyName] : content.name;
            }
        }
        if (!name) {
            name = contentType.name;
        }

        return name;
    }
}

export default new ContentNameProvider();