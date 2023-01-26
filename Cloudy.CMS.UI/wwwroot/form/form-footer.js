import { html, useContext, useState } from '../preact-htm/standalone.module.js';
import EntityContext from './entity-context.js';
import stateManager from '../data/state-manager.js';

const FormFooter = ({ entityType }) => {
  const [saving, setSaving] = useState();
  const { state } = useContext(EntityContext);

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
    <button class="btn btn-primary" type="button" disabled=${state.invalidFields.length || !stateManager.hasChanges(state) || saving} onClick=${save}>${saving ? 'Saving ...' : 'Save'}</button>
    <button class="btn btn-beta ms-auto" type="button" disabled=${!stateManager.hasChanges(state) || saving} onClick=${discard}>Discard changes</button>
  </div>
  `
};

export default FormFooter;