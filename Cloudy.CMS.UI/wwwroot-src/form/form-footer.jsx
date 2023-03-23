import html from '@src/html-init.js';
import { useContext, useState } from 'preact/hooks';
import EntityContext from './contexts/entity-context';
import stateManager from '../data/state-manager.js';
import ValidationManager from '../data/validation-manager.js';
import changeManager from '../data/change-manager.js';

const FormFooter = ({ validateAll }) => {
  const [saving, setSaving] = useState();
  const { state } = useContext(EntityContext);

  if(state.conflicts.length) {
    return;
  }


  const save = async () => {
    if (validateAll(state.entityReference)) {
      setSaving(true);
      await stateManager.save(state);
      setSaving(false);
    } else {
      setTimeout(() => window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' }), 100);
    }
  };

  const discard = async () => {
    changeManager.discardChanges(state);
    stateManager.replace(state);
    if (!state.new) {
      stateManager.reloadEntityForState(state.entityReference);
    }
  };

  return html`
  <div class="d-flex">
    <button class="btn btn-primary" type="button" disabled=${saving || state.conflicts.length} onClick=${save}>Save</button>
    <button class="btn btn-beta ms-auto" type="button" disabled=${!state.changes.length || saving} onClick=${discard}>Discard changes</button>
  </div>
    ${ValidationManager.anyIsInvalid(state.validationResults) ? html`
      <div class="alert alert-warning mt-3" role="alert">
        The form contains validation errors that need to be reviewed before proceeding.
      </div>` : null}
  `
};

export default FormFooter;