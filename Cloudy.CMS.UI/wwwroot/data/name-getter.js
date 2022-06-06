import getIntermediateSimpleValue from "../util/get-intermediate-simple-value.js";
import primaryKeyProvider from "./primary-key-provider.js";

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

    getNameOfState(state, contentType) {
        if (!state) {
            throw new Error('No state was supplied');
        }
        if (!contentType) {
            throw new Error('No content type was supplied');
        }

        let name = null;

        if (!contentType.isSingleton) {
            if (contentType.isNameable) {
                name = contentType.nameablePropertyName ?
                getIntermediateSimpleValue(state, [contentType.nameablePropertyName]) :
                getIntermediateSimpleValue(state, ['Name']);
            }
        }
        if (!name) {
            if (contentType.isSingleton) {
                name = contentType.name;
            } else if (state.contentReference.newContentKey) {
                name = `${contentType.name} ${state.contentReference.newContentKey}`;
            } else {
                name = `${contentType.name} ${JSON.stringify(state.contentReference.keyValues)}`;
            }
        }

        return name;
    }
}

export default new NameGetter();