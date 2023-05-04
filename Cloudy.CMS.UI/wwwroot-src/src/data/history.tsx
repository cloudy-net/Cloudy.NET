import EntityContext from "../form/contexts/entity-context";
import { useContext } from 'preact/hooks';
import changeManager from "./change-manager";
import diff from "./diff";
import Change from "./change";

const ViewChanges = () => {
  const { state } = useContext(EntityContext);

  const buildDiff = ([diffState, segment]: [diffState: number, segment: string]) => {
    if (diffState == diff.INSERT) {
      return <span class="cloudy-ui-diff-insert">{segment}</span>;
    }

    if (diffState == diff.DELETE) {
      return <span class="cloudy-ui-diff-delete">{segment}</span>;
    }

    return segment;
  };

  const showChange = (change: Change) => {
    const getDiff = () => {
      const initialValue = changeManager.getSourceValue(state.source.value, change.path);
      const result = (typeof initialValue == 'string' || initialValue == null) &&
        (typeof change.value == 'string' || change.value == null) ?
        diff(initialValue || '', change.value || '', 0).map(buildDiff) :
        change.value;
      return result;
    }

    return <>
      {change.path.split('.').map((p, i) => <>{i ? ' » ' : null} <span>{p}</span></>)}:
      {
        change.$type == 'simple' ? <> Changed to “{getDiff()}”</> :
        change.$type == 'embeddedblocklist.add' ? <> Added {change.type} “{change.key}”</> :
        change.$type == 'embeddedblocklist.remove' ? <> Removed {change.type} “{change.key}”</> :
        change.$type == 'blocktype' ? <> Changed block type to “{change.type}”</> :
          ` Unknown change type`
      }
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