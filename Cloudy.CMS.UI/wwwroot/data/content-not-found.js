class ContentNotFound extends Error {
    constructor(keyValues, contentTypeId) {
        super(`Content ${JSON.stringify(keyValues)} of type ${contentTypeId} not found`);
    }
}

export default ContentNotFound;