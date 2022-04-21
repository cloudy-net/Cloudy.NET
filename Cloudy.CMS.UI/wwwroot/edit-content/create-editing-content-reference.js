function createEditingContentReference(contentType) {
    // https://stackoverflow.com/questions/5092808/how-do-i-randomly-generate-html-hex-color-codes-using-javascript
    const newContentKey = (Math.random() * 0xFFFFFF << 0).toString(16).padStart(6, '0');
    return { newContentKey, contentTypeId: contentType.id };
}

export default createEditingContentReference;