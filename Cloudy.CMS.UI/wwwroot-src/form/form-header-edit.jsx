
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

const EditHeader = ({ entityTypeName, keyValues, fields }) => {
  const [instanceName, setInstanceName] = useState();
  const [editRoutes, setEditRoutes] = useState([]);
  const { entityReference, state } = useContext(EntityContext);
  const [saving, setSaving] = useState();

  useEffect(() => setInstanceName(simpleChangeHandler.getIntermediateValue(state, 'Name')), [entityReference]);

  useEffect(() => {
    (async () => await fetch(
      `/Admin/api/layout/content-routes?entityTypeName=${entityTypeName}&keys=${keyValues.join('&keys=')}`,
      { credentials: 'include' })
      .then(r => r.json())
      .then(r => setEditRoutes(r))
    )();
  }, []);

  if (state.conflicts.length) {
    return;
  }

  const save = async () => {
    if (ValidationManager.validateAll(fields, state.entityReference)) {
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
      <div className="form-breadcrumb">
        <a className="form-breadcrumb-item" href={`/Admin`}>Dashboard <Caret className="form-breadcrumb-caret" /></a>
        <a className="form-breadcrumb-item" href={`/Admin/List/${entityTypeName}`}>{entityTypeName} <Caret className="form-breadcrumb-caret" /></a>
        <a className="form-breadcrumb-item">{instanceName} <Caret className="form-breadcrumb-caret" /></a>
        <a className="form-breadcrumb-item active">Edit</a>
      </div>
      <div class="form-header-title-outer">
        <div className="form-header-title">{instanceName}</div>
        <a class="button text ml5" href={`/Admin/New/${entityTypeName}`}>New</a>&nbsp;
      </div>
    </div>
    <div className="form-header-buttons">
      <button class="button primary" type="button" disabled={saving || state.conflicts.length} onClick={save}>Save</button>
      <Dropdown contents="More" className={"button"}>
        <DropdownItem text="Discard changes" onClick={discard} disabled={!state.changes.length || saving} />
        {editRoutes.length === 1 && <DropdownItem text="🌎 View" href={editRoutes[0]} />}
        {editRoutes.length > 1 && <>
          <DropdownSeparator />
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