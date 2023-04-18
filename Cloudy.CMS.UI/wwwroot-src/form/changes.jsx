import Conflicts from "../data/conflicts";
import History from "../data/history";
import { useContext, useState } from 'preact/hooks';
import EntityContext from "./contexts/entity-context";
import ApplicationStateContext from "../application-state-context";


const Changes = () => {
  const { state } = useContext(EntityContext);
  const { showChanges } = useContext(ApplicationStateContext);

  if (state.conflicts.length) {
    return <>
      <div class="alert alert-info">
        <strong>The source and/or model has changed since you started editing.</strong><br />
        You must review these changes before you continue.
      </div>
      <Conflicts />
    </>;
  }

  if (state.changes.length == 0) {
    return <></>;
  }

  return <>
    {showChanges.value && <History />}
  </>;
};

export default Changes;