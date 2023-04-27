import History from "../data/history";
import { useContext, useState } from 'preact/hooks';
import EntityContext from "./contexts/entity-context";
import ApplicationStateContext from "../application-state-context";
import changeManager from "../data/change-manager";
import stateManager from "../data/state-manager";


const Changes = () => {
  const { state } = useContext(EntityContext);
  const { showChanges } = useContext(ApplicationStateContext);

  if (state.newSource) {
    const discard = () => {
      changeManager.discardChanges(state);
      stateManager.replace(state);
      stateManager.reloadEntityForState(state.entityReference);
    }

    return <div>
      <div class="alert">
        <strong>The source and/or model has changed since you started editing.</strong><br />
        <div>You will need to discard your changes.</div>
      </div>
      <History />
      <a className="button button-primary" onClick={discard}>Discard changes</a>
    </div>;
  }

  if (state.changes.length == 0) {
    return <></>;
  }

  return <>
    {showChanges.value && <History />}
  </>;
};

export default Changes;