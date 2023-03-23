import EntityContext from "../form/contexts/entity-context";
import { useContext } from 'preact/hooks';
import changeManager from "./change-manager.js";
import diff from "./diff.js";

const ViewChanges = () => {
  const { state } = useContext(EntityContext);

  const buildDiff = ([state, segment]) => {
    if (state == diff.INSERT) {
      return <span class="cloudy-ui-diff-insert">{segment}</span>;
    }

    if (state == diff.DELETE) {
      return <span class="cloudy-ui-diff-delete">{segment}</span>;
    }

    return segment;
  };

  const showChange = change => {
    const initialValue = changeManager.getSourceValue(state.source.value, change.path);
    const result = (typeof initialValue == 'string' || initialValue == null) &&
      (typeof change.value == 'string' || change.value == null) ?
      diff(initialValue || '', change.value || '', 0).map(buildDiff) :
      change.value;

    return <>
      {change.path.split('.').map((p, i) => <>{i ? ' » ' : null} <span>{p}</span></>)}:
      {change.$type == 'simple' ? ` Changed to “${result}”` : ` Changed block type to “${change.type}”`}
    </>;
  };

  return <>
    <p><strong>Your changes:</strong></p>
    <ul>
      {state.changes.map(change => <li>{showChange(change)}</li>)}
    </ul>
  </>;
};

export default ViewChanges;