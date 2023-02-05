import EntityContext from "../form/entity-context.js";
import { html, useContext, useState } from "../preact-htm/standalone.module.js";
import diff from "./diff.js";
import showConflict from "./show-conflict.js";
import stateManager from "./state-manager.js";

const buildDiff = ([state, segment]) => {
  if (state == diff.INSERT) {
    return html`<span class=cloudy-ui-diff-insert>${segment}</span>`;
  }

  if (state == diff.DELETE) {
    return html`<span class=cloudy-ui-diff-delete>${segment}</span>`;
  }

  return segment;
};

const ViewChanges = () => {
  const { state, mergedChanges, sourceConflicts } = useContext(EntityContext);
  const [actions, setActions] = useState({});

  const showChange = change => {
    const initialValue = stateManager.getSourceValue(state.source.value, change.path);
    const result = (typeof initialValue == 'string' || initialValue == null) &&
      (typeof change.value == 'string' || change.value == null) ?
      diff(initialValue || '', change.value || '', 0).map(buildDiff) :
      change.value;

    return html`
      ${change.path.split('.').map((p, i) => html`${i ? ' » ' : null} <span>${p}</span>`)}:
      ${change['$type'] == 'simple' ? html` Changed to “${result}”` : ` Changed block type to “${change.type}”`}
    `
  };

  if (sourceConflicts.length) {
    const discardConflicts = () => {
      stateManager.discardSourceConflicts(state, sourceConflicts);
    };

    return html`<div class="m-3">
      <p><strong>Conflicting source and/or model changes:</strong></p>
      <table class="table">
        <thead>
          <tr><th>Property<//><th>Source<//><th>Your changes<//><th>Action<//><//>
        <//>
        <tbody>
          ${sourceConflicts.map(conflict => html`<${showConflict} conflict=${conflict} setAction=${(path, action) => setActions({ ...actions, [path]: action })}/>`)}
        <//>
      <//>
      <p><button class="btn btn-primary" type="button" onClick=${() => discardConflicts()}>Discard incompatible changes</button></p>
      <//>`
  }

  return html`
    <p><strong>Your changes:</strong></p>
    <ul>
      ${mergedChanges.map(change => html`<li>${showChange(change)}</li>`)}
    </ul>
    `
  // <p><button class="btn btn-primary" type="button">Discard incompatible changes</button></p>
};

export default ViewChanges;