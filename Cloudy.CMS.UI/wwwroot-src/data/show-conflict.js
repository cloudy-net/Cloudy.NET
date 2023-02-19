import html from '@src/html-init.js';
import { useContext } from 'preact/hooks';
import EntityContext from "../form/entity-context.js";
import changeManager from "./change-manager.js";
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

const ShowConflict = ({ conflict, actions, setAction }) => {
  const { state } = useContext(EntityContext);
  const change = state.changes.find(change => change.path == conflict.path);

  if (!change) { // sourceConflicts and changes get briefly out of sync when clearing
    return;
  }

  const initialValue = changeManager.getSourceValue(state.source.value, change.path);
  const result = (typeof initialValue == 'string' || initialValue == null) &&
    (typeof change.value == 'string' || change.value == null) ?
    diff(initialValue || '', change.value || '', 0).map(buildDiff) :
    change.value;

  const path = change.path.split('.').map((p, i) => html`${i ? ' » ' : null} <span>${p}</span>`);

  if (conflict.type == 'deleted') {
    return html`<tr class="align-middle"><td>${path}<//><td>Property deleted<//><td>${result}<//><td>Will be deleted<//><//>`;
  }
  if (conflict.type == 'blockdeleted') {
    return html`<tr class="align-middle"><td>${path}<//><td>Block deleted<//><td>${result}<//><td>Will be deleted<//><//>`;
  }
  if (conflict.type == 'pendingchangesourceconflict') {
    const newValue = changeManager.getSourceValue(state.newSource.value, change.path);
    const sourceResult = (typeof initialValue == 'string' || initialValue == null) &&
      (typeof newValue == 'string' || newValue == null) ?
      diff(initialValue || '', newValue || '', 0).map(buildDiff) :
      newValue;

    const select = html`<select class="form-control form-control-sm" value=${actions[change.path]} onChange=${event => setAction(change.path, event.target.value)}>
      <option value="" disabled selected hidden>Select action<//>
      <option value="keep-source">Keep source<//>
      <option value="keep-changes">Keep changes<//>
    <//>`;

    return html`<tr class="align-middle"><td>${path}<//><td class=${actions[change.path] == 'keep-changes' ? 'strikethrough' : null}>${sourceResult}<//><td class=${actions[change.path] == 'keep-source' ? 'strikethrough' : null}>${result}<//><td>${select}<//><//>`;
  }

  return '<tr class="align-middle"><td>(Unknown change type)<//><//>';
};

export default ShowConflict;