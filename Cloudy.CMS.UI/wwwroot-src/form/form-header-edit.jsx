
import { useState, useEffect, useContext } from 'preact/hooks';
import EntityContext from '@src/form/contexts/entity-context';
import simpleChangeHandler from '@src/data/change-handlers/simple-change-handler.js';
import { ReactComponent as Caret } from "../assets/caret-horizontal.svg";
import stateManager from '../data/state-manager.js';
import ValidationManager from '../data/validation-manager.js';
import changeManager from '../data/change-manager.js';
import Dropdown from '../components/dropdown';
import DropdownItem from '../components/dropdown-item';
import DropdownSeparator from '../components/dropdown-separator';
import ApplicationStateContext from '../application-state-context';

const EditHeader = ({ entityTypeName, keyValues }) => {
  const [instanceName, setInstanceName] = useState();
  const [editRoutes, setEditRoutes] = useState([]);
  const { entityReference, state } = useContext(EntityContext);
  const [saving, setSaving] = useState();
  const { showChanges, fieldTypes } = useContext(ApplicationStateContext);

  useEffect(() => setInstanceName(simpleChangeHandler.getIntermediateValue(state, 'Name')), [entityReference]);

  useEffect(() => {
    (async () => await fetch(
      `/Admin/api/layout/content-routes?entityTypeName=${entityTypeName}&keys=${keyValues.join('&keys=')}`,
      { credentials: 'include' })
      .then(r => r.json())
      .then(r => setEditRoutes(r))
    )();
  }, []);

  if (state.newSource) {
    return;
  }

  const save = async () => {
    if (ValidationManager.validateAll(fieldTypes.value[entityTypeName], state.entityReference)) {
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

  return <div className="form-header">
    <div className="form-header-body">
      <div className="breadcrumb">
        <a className="breadcrumb-item" href={`/Admin`}>Dashboard <Caret className="breadcrumb-caret" /></a>
        <a className="breadcrumb-item" href={`/Admin/List/${entityTypeName}`}>{entityTypeName} <Caret className="breadcrumb-caret" /></a>
        <a className="breadcrumb-item">{instanceName} <Caret className="breadcrumb-caret" /></a>
        <a className="breadcrumb-item active">Edit</a>
      </div>
      <div className="form-header-title">{instanceName}</div>
    </div>
    <div className="form-header-buttons">
      <button class="button primary" type="button" disabled={saving || state.newSource} onClick={save}>Save</button>
      <Dropdown contents="More" className={"button"}>
        <DropdownItem text={showChanges.value ? "Hide changes" : "Show changes"} onClick={() => showChanges.value = !showChanges.value} disabled={!state.changes.length || saving} />
        <DropdownItem text="Discard changes" onClick={discard} disabled={!state.changes.length || saving} />
        <DropdownSeparator text="Browse" />
        {editRoutes.length === 1 && <DropdownItem text="🌎 View" href={editRoutes[0]} />}
        {editRoutes.length > 1 && <>
          {editRoutes.map(url => <DropdownItem text={`🌎 ${url}`} href={url} />)}
        </>}
      </Dropdown>
    </div>
    {ValidationManager.anyIsInvalid(state.validationResults) &&
      <div class="alert alert-warning mt-3" role="alert">
        The form contains validation errors that need to be reviewed before proceeding.
      </div>}
  </div>
};

export default EditHeader;