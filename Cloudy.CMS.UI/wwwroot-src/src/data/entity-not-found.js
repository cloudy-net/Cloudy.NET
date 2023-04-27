class EntityNotFound extends Error {
    constructor(entityReference) {
        super(`Entity ${JSON.stringify(entityReference.keyValues)} of type ${entityReference.entityTypeId} not found`);
    }
}

export default EntityNotFound;