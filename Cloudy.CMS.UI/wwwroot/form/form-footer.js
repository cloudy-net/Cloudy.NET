import { html, useContext, useState } from '../preact-htm/standalone.module.js';
import EntityContext from './entity-context.js';
import stateManager from '../data/state-manager.js';
import ValidationManager from '../data/validation-manager.js';

const FormFooter = ({ validateAll }) => {
  const [saving, setSaving] = useState();
  const { state } = useContext(EntityContext);

  const save = async () => {
    if (validateAll(state.entityReference)) {
      setSaving(true);
      await stateManager.save([state.entityReference]);
      setSaving(false);
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
    <button class="btn btn-primary" type="button" disabled=${ValidationManager.anyIsInvalid(state.validationResults) || !stateManager.hasChanges(state) || saving} onClick=${save}>${saving ? 'Saving ...' : 'Save'}</button>
    <button class="btn btn-beta ms-auto" type="button" disabled=${!stateManager.hasChanges(state) || saving} onClick=${discard}>Discard changes</button>
  </div>
  ${ValidationManager.anyIsInvalid(state.validationResults) ? html`<p style="color:red">Fix errors</p>` : null }
  `
};

export default FormFooter;