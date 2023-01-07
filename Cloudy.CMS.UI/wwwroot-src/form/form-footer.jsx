import { useContext, useState } from '@preact-htm';
import FormField from './form-field';
import EntityContext from './entity-context';
import stateManager from '../data/state-manager';

const FormFooter = ({ contentType }) => {
  const [saving, setSaving] = useState();
  const { state } = useContext(EntityContext);

  const save = async () => {
    setSaving(true);

    await stateManager.save([state.contentReference]);
    
    setSaving(false);
  };

  return <><button class="btn btn-primary" type="button" disabled={!state.simpleChanges.length || saving} onClick={save}>{saving ? 'Saving ...' : 'Save'}</button></>
};

export default FormFooter;