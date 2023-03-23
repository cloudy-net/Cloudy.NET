import EntityContext from "../form/contexts/entity-context";
import { useContext, useState  } from 'preact/hooks';
import conflictManager from "./conflict-manager.js";
import ShowConflict from "./show-conflict";
import stateManager from "./state-manager.js";

const Conflicts = () => {
  const { state } = useContext(EntityContext);
  const [actions, setActions] = useState({});
  const [message, setMessage] = useState();

  const resolve = () => {
    if (state.conflicts.filter(conflict => conflict.type == "pendingchangesourceconflict" && !actions[conflict.path]).length) {
      setMessage('Please select actions for all conflicts');
      return;
    }

    const newState = conflictManager.resolveConflicts(state, actions);

    if (!newState) {
      setMessage('All conflicts could not be resolved.');
      return;
    }

    stateManager.replace(newState);

    setMessage('Resolved conflicts.');
  };

  return <div class="m-3">
        <p><strong>Conflicting source and/or model changes:</strong></p>
        <table class="table">
          <thead>
            <tr><th>Property</th><th>Source</th><th>Your changes</th><th>Action</th></tr>
          </thead>
          <tbody>
            ${state.conflicts.map(conflict => <ShowConflict conflict={conflict} actions={actions} setAction={(path, action) => setActions({ ...actions, [path]: action })}/>)}
          </tbody>
        </table>
        <p>
          <button class="btn btn-primary me-2" type="button" onClick={() => resolve()}>Apply</button>
          {message && <div class="d-inline-block ms-2">{message}</div>}
        </p>
      </div>;
};

export default Conflicts;