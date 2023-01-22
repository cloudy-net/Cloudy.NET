import { useContext, useState } from '../preact-htm/standalone.module';
import FormField from './form-field';
import EntityContext from './entity-context';
import stateManager from '../data/state-manager';

const FormFooter = ({ entityType }) => {
  const [saving, setSaving] = useState();
  const { state } = useContext(EntityContext);

  const save = async () => {
    setSaving(true);

    await stateManager.save([state.contentReference]);
    
    setSaving(false);
  };

  return <><button class="btn btn-primary" type="button" disabled={!stateManager.hasChanges(state) || saving} onClick={save}>{saving ? 'Saving ...' : 'Save'}</button></>
};

export default FormFooter;