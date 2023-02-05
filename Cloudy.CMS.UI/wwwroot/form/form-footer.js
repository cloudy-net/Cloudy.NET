import { html, useContext, useState } from '../preact-htm/standalone.module.js';
import EntityContext from './entity-context.js';
import stateManager from '../data/state-manager.js';
import ValidationManager from '../data/validation-manager.js';

const FormFooter = ({ validateAll }) => {
  const [saving, setSaving] = useState();
  const { state, mergedChanges, sourceConflicts } = useContext(EntityContext);

  const save = async () => {
    if (validateAll(state.entityReference)) {
      setSaving(true);
      await stateManager.save([state.entityReference]);
      setSaving(false);
    } else {
      setTimeout(() => window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' }), 100);
    }
  };

  const discard = async () => {
    stateManager.replace({ ...state, changes: [] });
    if (state.entityReference.keyValues) {
      stateManager.reloadContentForState(state.entityReference);
    }
  };

  return html`
  <div class="d-flex">
    <button class="btn btn-primary" type="button" disabled=${saving || sourceConflicts.length} onClick=${save}>${saving ? 'Saving ...' : 'Save'}</button>
    <button class="btn btn-beta ms-auto" type="button" disabled=${!mergedChanges.length || saving} onClick=${discard}>Discard changes</button>
  </div>
    ${ValidationManager.anyIsInvalid(state.validationResults) ? html`
      <div class="alert alert-warning mt-3" role="alert">
        The form contains validation errors that need to be reviewed before proceeding.
      </div>` : null}
  `
};

export default FormFooter;