import { html, useContext, useState } from '../preact-htm/standalone.module.js';
import EntityContext from './entity-context.js';
import stateManager from '../data/state-manager.js';

const FormFooter = ({ entityType }) => {
  const [saving, setSaving] = useState();
  const { state } = useContext(EntityContext);

  const save = async () => {
    setSaving(true);

    await stateManager.save([state.contentReference]);
    
    setSaving(false);
  };

  return html`<button class="btn btn-primary" type="button" disabled=${!stateManager.hasChanges(state) || saving} onClick=${save}>${saving ? 'Saving ...' : 'Save'}</button>`
};

export default FormFooter;