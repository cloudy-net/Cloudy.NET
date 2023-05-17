import Change from "./change";
import EntityReference from "./entity-reference";
import Source from "./source";

type State = {
  new?: boolean,
  nameHint?: string | null,
  history: Change[];
  entityReference: EntityReference,
  changes: Change[],
  loading?: boolean,
  source: Source | null,
  loadingNewSource?: boolean,
  newSource?: Source | null,
  validationResults: any[],
};

export default State;