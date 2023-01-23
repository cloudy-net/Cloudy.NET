class ContentNotFound extends Error {
    constructor(contentReference) {
        super(`Content ${JSON.stringify(contentReference.keyValues)} of type ${contentReference.entityTypeId} not found`);
    }
}

export default ContentNotFound;