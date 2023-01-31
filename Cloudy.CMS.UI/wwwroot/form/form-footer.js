import { html, useContext, useState } from '../preact-htm/standalone.module.js';
import EntityContext from './entity-context.js';
import stateManager from '../data/state-manager.js';

const FormFooter = () => {
  const [saving, setSaving] = useState();
  const { state, mergedChanges, modelConflicts } = useContext(EntityContext);

  const save = async () => {
    setSaving(true);

    await stateManager.save([state.entityReference]);

    setSaving(false);
  };

  const discard = async () => {
    stateManager.replace({ ...state, changes: [] });
    if (state.entityReference.keyValues) {
      stateManager.reloadContentForState(state.entityReference);
    }
  };

  return html`
  <div class="d-flex">
    <button class="btn btn-primary" type="button" disabled=${saving || modelConflicts.length} onClick=${save}>${saving ? 'Saving ...' : 'Save'}</button>
    <button class="btn btn-beta ms-auto" type="button" disabled=${!mergedChanges.length || saving} onClick=${discard}>Discard changes</button>
  </div>
  `
};

export default FormFooter;