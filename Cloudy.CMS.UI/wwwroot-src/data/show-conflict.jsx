import { useContext } from 'preact/hooks';
import EntityContext from "../form/contexts/entity-context";
import changeManager from "./change-manager.js";
import diff from "./diff.js";

const buildDiff = ([state, segment]) => {
  if (state == diff.INSERT) {
    return <span class="cloudy-ui-diff-insert">{segment}</span>;
  }

  if (state == diff.DELETE) {
    return <span class="cloudy-ui-diff-delete">{segment}</span>;
  }

  return segment;
};

const ShowConflict = ({ conflict, actions, setAction }) => {
  const { state } = useContext(EntityContext);
  const change = state.changes.find(change => change.path == conflict.path);

  if (!change) { // sourceConflicts and changes get briefly out of sync when clearing
    return;
  }

  const initialValue = changeManager.getSourceValue(state.source.value, change.path);
  const result = (typeof initialValue == 'string' || initialValue == null) &&
    (typeof change.value == 'string' || change.value == null) ?
    diff(initialValue || '', change.value || '', 0).map(buildDiff) :
    change.value;

  const path = change.path.split('.').map((p, i) => <>{i ? ' » ' : null} <span>{p}</span></>);

  if (conflict.type == 'deleted') {
    return <tr class="align-middle"><td>{path}</td><td>Property deleted</td><td>{result}</td><td>Will be deleted</td></tr>;
  }
  if (conflict.type == 'blockdeleted') {
    return <tr class="align-middle"><td>{path}</td><td>Block deleted</td><td>{result}</td><td>Will be deleted</td></tr>;
  }
  if (conflict.type == 'pendingchangesourceconflict') {
    const newValue = changeManager.getSourceValue(state.newSource.value, change.path);
    const sourceResult = (typeof initialValue == 'string' || initialValue == null) &&
      (typeof newValue == 'string' || newValue == null) ?
      diff(initialValue || '', newValue || '', 0).map(buildDiff) :
      newValue;

    const select = <select class="form-control form-control-sm" value={actions[change.path]} onChange={event => setAction(change.path, event.target.value)}>
      <option value="" disabled selected hidden>Select action</option>
      <option value="keep-source">Keep source</option>
      <option value="keep-changes">Keep changes</option>
    </select>;

    return <tr class="align-middle"><td>{path}</td><td class={actions[change.path] == 'keep-changes' ? 'strikethrough' : null}>{sourceResult}</td><td class={actions[change.path] == 'keep-source' ? 'strikethrough' : null}>{result}</td><td>{select}</td></tr>;
  }

  return <tr class="align-middle"><td>(Unknown change type)</td></tr>;
};

export default ShowConflict;