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
  const { state, mergedChanges, modelConflicts } = useContext(EntityContext);

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
    const change = mergedChanges.find(change => change.path == conflict.name);

    if (!change) { // modelConflicts and changes get briefly out of sync when clearing
      return;
    }

    const initialValue = stateManager.getSourceValue(state.source.value, change.path);
    const result = (typeof initialValue == 'string' || initialValue == null) &&
      (typeof change.value == 'string' || change.value == null) ?
      diff(initialValue || '', change.value || '', 0).map(buildDiff) :
      change.value;

    return html`
      ${change.path.split('.').map((p, i) => html`${i ? ' » ' : null} <span>${p}</span>`)} was ${conflict.type}:
      ${change['$type'] == 'simple' ? html` Was “${result}”` : ` Block type was “${change.type}”`}
    `
  };

  if (modelConflicts.length) {
    const discardConflicts = () => {
      stateManager.discardSourceConflicts(state, modelConflicts);
    };

    return html`
      <p><strong>Deleted properties with unsaved changes:</strong></p>
      <ul>
        ${modelConflicts.map(conflict => showConflict(conflict))}
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