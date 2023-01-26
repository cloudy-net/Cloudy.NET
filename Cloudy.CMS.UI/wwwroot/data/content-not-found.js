class ContentNotFound extends Error {
    constructor(entityReference) {
        super(`Content ${JSON.stringify(entityReference.keyValues)} of type ${entityReference.entityTypeId} not found`);
    }
}

export default ContentNotFound;