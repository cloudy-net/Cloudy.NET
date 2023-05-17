import EntityReference from "./entity-reference";

class EntityNotFound extends Error {
    constructor(entityReference: EntityReference) {
        super(`Entity ${JSON.stringify(entityReference.keyValues)} of type ${entityReference.entityType} not found`);
    }
}

export default EntityNotFound;