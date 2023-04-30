import Change from "./change";
import EntityReference from "./entity-reference";
import Source from "./source";

type State = {
  new: boolean,
  history: Change[] | null;
  entityReference: EntityReference,
  changes: Change[] | null,
  loading?: boolean,
  source: Source | null,
  newSource?: Source | null,
  validationResults: any[],
};

export default State;