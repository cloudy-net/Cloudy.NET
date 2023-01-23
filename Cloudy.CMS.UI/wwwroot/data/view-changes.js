import EntityContext from "../form/entity-context.js";
import { html, useContext, useState } from "../preact-htm/standalone.module.js";
import stateManager from "./state-manager.js";

const ViewChanges = () => {
  const { state } = useContext(EntityContext);

  const referenceChanges = stateManager.getReferenceChanges(state);

  const showChange = change => {
    return html`
      ${change.path.map((p, i) => html`${i ? ' » ' : null} <span>${p}</span>`)}: ${change['$type'] == 'simple' ? `Changed to “${change.value}”` : `Changed block type to “${change.type}”`}
    `
  };
  
  return html`
    <p><strong>Your changes:</strong></p>
    <ul>
      {stateManager.getMergedChanges(state).map(change =>
        <li>{showChange(change)}</li>
      )}
    </ul>
    <p><button class="btn btn-primary" type="button">Discard incompatible changes</button></p>
  `
};

export default ViewChanges;