import primaryKeyProvider from "./primary-key-provider.js";
import contentTypeProvider from "./content-type-provider.js";



/* CONTENT NAME PROVIDER */

class ContentNameProvider {
    async getNameOf(content, contentTypeId) {
        if (!content) {
            throw new Error('No content was supplied');
        }
        if (!contentTypeId) {
            throw new Error('No content type id was supplied');
        }

        var contentType = await contentTypeProvider.get(contentTypeId);

        var name = '';

        if (!contentType.isSingleton) {
            if (contentType.isNameable) {
                name = contentType.nameablePropertyName ? content[contentType.nameablePropertyName] : content.name;
            }
        }
        if (!name) {
            const contentId = await primaryKeyProvider.getFor(content, contentType);
            if (contentId && !contentType.isSingleton) {
                name = `${contentType.name} ${JSON.stringify(contentId)}`;
            } else {
                name = contentType.name;
            }
        }

        return name;
    }
}

export default new ContentNameProvider();