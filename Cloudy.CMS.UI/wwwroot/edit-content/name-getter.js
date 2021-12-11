import primaryKeyProvider from "../ContentAppSupport/utils/primary-key-provider.js";

class NameGetter {
    getNameOf(content, contentType) {
        if (!content) {
            throw new Error('No content was supplied');
        }
        if (!contentType) {
            throw new Error('No content type was supplied');
        }

        let name = null;

        if (!contentType.isSingleton) {
            if (contentType.isNameable) {
                name = contentType.nameablePropertyName ? content[contentType.nameablePropertyName] : content.name;
            }
        }
        if (!name) {
            const contentId = primaryKeyProvider.getFor(content, contentType);
            if (contentId && !contentType.isSingleton) {
                name = `${contentType.name} ${JSON.stringify(contentId)}`;
            } else {
                name = contentType.name;
            }
        }

        return name;
    }
}

export default new NameGetter();