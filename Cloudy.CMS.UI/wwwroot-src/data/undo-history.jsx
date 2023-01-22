import EntityContext from "../form/entity-context";
import { useContext, useState } from "../preact-htm/standalone.module";

const UndoHistory = () => {
  const { state } = useContext(EntityContext);

  const showChange = change => {
    return <>
      {change.path.map((p, i) => <>{i ? ' » ' : null} <span>{p}</span></>)}: {change['$type'] == 'simple' ? `Changed to “${change.value}”` : `Changed block type to “${change.type}”`}
    </>
  };
  
  return <>
    <p><strong>Your changes:</strong></p>
    <ul>
      {state.changes.map(change =>
        <li>{showChange(change)}</li>
      )}
    </ul>
    <p><button class="btn btn-primary" type="button">Discard incompatible changes</button></p>
  </>
};

export default UndoHistory;