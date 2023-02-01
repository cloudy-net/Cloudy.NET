import EntityContext from "../form/entity-context.js";
import { html, useContext, useState } from "../preact-htm/standalone.module.js";
import diff from "./diff.js";
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

const getChangeBadge = () => {
  if (!initialValue && value) {
    return html`<cloudy-ui-form-field-change-badge class=cloudy-ui-added title="This value has been added."><//>`;
  }

  if (initialValue && !value) {
    return html`<cloudy-ui-form-field-change-badge class=cloudy-ui-removed title="This value has been removed."><//>`;
  }

  if (initialValue != value) {
    return html`<cloudy-ui-form-field-change-badge class=cloudy-ui-modified title="This value has been modified."><//>`;
  }
};

const ViewChanges = () => {
  const { state, mergedChanges, sourceConflicts } = useContext(EntityContext);

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

  const showConflict = conflict => {
    const change = mergedChanges.find(change => change.path == conflict.path);

    if (!change) { // sourceConflicts and changes get briefly out of sync when clearing
      return;
    }

    const initialValue = stateManager.getSourceValue(state.source.value, change.path);
    const result = (typeof initialValue == 'string' || initialValue == null) &&
      (typeof change.value == 'string' || change.value == null) ?
      diff(initialValue || '', change.value || '', 0).map(buildDiff) :
      change.value;

      const path = change.path.split('.').map((p, i) => html`${i ? ' » ' : null} <span>${p}</span>`);

    if (conflict.type == 'deleted') {
      return html`${path}: Was deleted`;
    }
    if (conflict.type == 'blockdeleted') {
      return html`${path}: Block type was changed”`;
    }
    if (conflict.type == 'pendingchangesourceconflict') {
      const newValue = stateManager.getSourceValue(state.newSource.value, change.path);
      const sourceResult = (typeof initialValue == 'string' || initialValue == null) &&
        (typeof newValue == 'string' || newValue == null) ?
        diff(initialValue || '', newValue || '', 0).map(buildDiff) :
        newValue;
      return html`${path}: Was changed to “${sourceResult}”, you changed it to “${result}”`;
    }

    return '(Unknown change type)';
  };

  if (sourceConflicts.length) {
    const discardConflicts = () => {
      stateManager.discardSourceConflicts(state, sourceConflicts);
    };

    console.log(sourceConflicts);

    return html`
      <p><strong>Conflicting source and/or model changes:</strong></p>
      <ul>
        ${sourceConflicts.map(conflict => showConflict(conflict))}
      </ul>
      <p><button class="btn btn-primary" type="button" onClick=${() => discardConflicts()}>Discard incompatible changes</button></p>
      `
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