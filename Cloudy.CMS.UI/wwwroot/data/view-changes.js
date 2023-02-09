import EntityContext from "../form/entity-context.js";
import { html, useContext, useState } from "../preact-htm/standalone.module.js";
import changeManager from "./change-manager.js";
import conflictManager from "./conflict-manager.js";
import diff from "./diff.js";
import ShowConflict from "./show-conflict.js";
import stateManager from "./state-manager.js";

const ViewChanges = () => {
  const { state, mergedChanges, sourceConflicts, clearMergedChanges, clearSourceConflicts } = useContext(EntityContext);
  const [actions, setActions] = useState({});
  const [message, setMessage] = useState();

  const applyReconciliation = () => {
    if (sourceConflicts.filter(conflict => conflict.type == 'pendingchangesourceconflict' && !actions[conflict.path]).length) {
      setMessage('Please select actions for all conflicts');
      return;
    }

    clearMergedChanges();
    clearSourceConflicts();
    conflictManager.discardSourceConflicts(state, sourceConflicts, actions);

    setMessage('Applied actions and updated source.');
  };

  if (sourceConflicts.length) {
    return html`<div class="m-3">
        <p><strong>Conflicting source and/or model changes:</strong></p>
        <table class="table">
          <thead>
            <tr><th>Property<//><th>Source<//><th>Your changes<//><th>Action<//><//>
          <//>
          <tbody>
            ${sourceConflicts.map(conflict => html`<${ShowConflict} conflict=${conflict} actions=${actions} setAction=${(path, action) => setActions({ ...actions, [path]: action })}/>`)}
          <//>
        <//>
        <p>
          <button class="btn btn-primary me-2" type="button" onClick=${() => applyReconciliation()}>Apply</button>
          <!--
          <button class="btn btn-beta me-2" type="button" onClick=${() => {
            const actions = {};

            for(let conflict of sourceConflicts.filter(conflict => conflict.type == 'pendingchangesourceconflict')){
              actions[conflict.path] = 'keep-source';
            }

            setActions(actions);
          }}>Discard all changes</button>
          <button class="btn btn-beta" type="button" onClick=${() => {
            const actions = {};

            for(let conflict of sourceConflicts.filter(conflict => conflict.type == 'pendingchangesourceconflict')){
              actions[conflict.path] = '';
            }

            setActions(actions);
          }}>Clear</button>
          -->
          ${message && html`<div class="d-inline-block ms-2">${message}<//>`}
        </p>
      <//>`
  }

  const buildDiff = ([state, segment]) => {
    if (state == diff.INSERT) {
      return html`<span class=cloudy-ui-diff-insert>${segment}</span>`;
    }

    if (state == diff.DELETE) {
      return html`<span class=cloudy-ui-diff-delete>${segment}</span>`;
    }

    return segment;
  };

  const showChange = change => {
    const initialValue = changeManager.getSourceValue(state.source.value, change.path);
    const result = (typeof initialValue == 'string' || initialValue == null) &&
      (typeof change.value == 'string' || change.value == null) ?
      diff(initialValue || '', change.value || '', 0).map(buildDiff) :
      change.value;

    return html`
      ${change.path.split('.').map((p, i) => html`${i ? ' » ' : null} <span>${p}</span>`)}:
      ${change['$type'] == 'simple' ? html` Changed to “${result}”` : ` Changed block type to “${change.type}”`}
    `
  };

  return html`
    <p><strong>Your changes:</strong></p>
    <ul>
      ${mergedChanges.map(change => html`<li>${showChange(change)}</li>`)}
    </ul>
    `
  // <p><button class="btn btn-primary" type="button">Discard incompatible changes</button></p>
};

export default ViewChanges;