import EntityContext from "../form/entity-context.js";
import html from '@src/html-init.js';
import { useContext, useState  } from 'preact/hooks';
import conflictManager from "./conflict-manager.js";
import ShowConflict from "./show-conflict.js";
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

  return html`<div class="m-3">
        <p><strong>Conflicting source and/or model changes:</strong></p>
        <table class="table">
          <thead>
            <tr><th>Property<//><th>Source<//><th>Your changes<//><th>Action<//><//>
          <//>
          <tbody>
            ${state.conflicts.map(conflict => html`<${ShowConflict} conflict=${conflict} actions=${actions} setAction=${(path, action) => setActions({ ...actions, [path]: action })}/>`)}
          <//>
        <//>
        <p>
          <button class="btn btn-primary me-2" type="button" onClick=${() => resolve()}>Apply</button>
          <!--
          <button class="btn btn-beta me-2" type="button" onClick=${() => {
      const actions = {};

      for (let conflict of state.conflicts.filter(conflict => conflict.type == 'pendingchangesourceconflict')) {
        actions[conflict.path] = 'keep-source';
      }

      setActions(actions);
    }}>Discard all changes</button>
          <button class="btn btn-beta" type="button" onClick=${() => {
      const actions = {};

      for (let conflict of state.conflicts.filter(conflict => conflict.type == 'pendingchangesourceconflict')) {
        actions[conflict.path] = '';
      }

      setActions(actions);
    }}>Clear</button>
          -->
          ${message && html`<div class="d-inline-block ms-2">${message}<//>`}
        </p>
      <//>`;
};

export default Conflicts;