const generateNewContentKey = () => (Math.random() * 0xFFFFFF << 0).toString(16).padStart(6, '0'); // https://stackoverflow.com/questions/5092808/how-do-i-randomly-generate-html-hex-color-codes-using-javascript

class ContentStateManager {
    createNewContent(contentType) {
        const contentReference = { newContentKey: generateNewContentKey(), keys: null, contentTypeId: contentType.id };
        return {
            ...contentReference,
            values: {}
        };
    };
}

export default new ContentStateManager();