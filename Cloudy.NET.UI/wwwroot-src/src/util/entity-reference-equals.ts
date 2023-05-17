import EntityReference from "../data/entity-reference";
import arrayEquals from "./array-equals";

const entityReferenceEquals = (a: EntityReference, b: EntityReference) => 
arrayEquals(a.keyValues, b.keyValues) && a.newEntityKey == b.newEntityKey && a.entityType == b.entityType;

export default entityReferenceEquals;