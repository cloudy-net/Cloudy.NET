
import { useState, useEffect, useContext } from 'preact/hooks';
import EntityContext from '../form/contexts/entity-context';
import simpleChangeHandler from '../data/change-handlers/simple-change-handler';
import Caret from "../assets/caret-horizontal.svg";
import stateManager from '../data/state-manager';
import ValidationManager from '../data/validation-manager';
import changeManager from '../data/change-manager';
import Dropdown from '../components/dropdown';
import DropdownItem from '../components/dropdown-item';
import DropdownSeparator from '../components/dropdown-separator';
import ApplicationStateContext from '../application-state-context';

const EditHeader = ({ entityTypeName, keyValues }: { entityTypeName: string, keyValues: string[] }) => {
  const [instanceName, setInstanceName] = useState();
  const [editRoutes, setEditRoutes] = useState([]);
  const { entityReference, state } = useContext(EntityContext);
  const [saving, setSaving] = useState(false);
  const { showChanges, fieldTypes } = useContext(ApplicationStateContext);

  useEffect(() => setInstanceName(simpleChangeHandler.getIntermediateValue(state.value!, 'Name')), [entityReference]);

  useEffect(() => {
    (async () => await fetch(
      `/Admin/api/layout/content-routes?entityTypeName=${entityTypeName}&keys=${keyValues.join('&keys=')}`,
      { credentials: 'include' })
      .then(r => r.json())
      .then(r => setEditRoutes(r))
    )();
  }, []);

  if (state.value!.newSource) {
    return <></>;
  }

  const save = async () => {
    if (ValidationManager.validateAll(fieldTypes.value![entityTypeName], entityReference.value!)) {
      setSaving(true);
      await stateManager.save(state.value!);
      setSaving(false);
    } else {
      setTimeout(() => window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' }), 100);
    }
  };

  const discard = async () => {
    changeManager.discardChanges(state.value!);
    stateManager.replace(state.value!);
    if (!state.value!.new) {
      stateManager.reloadEntityForState(entityReference.value!);
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
      <button class="button primary" type="button" disabled={saving || !!state.value!.newSource} onClick={save}>Save</button>
      <Dropdown contents="More" className={"button"}>
        <DropdownItem text={showChanges.value ? "Hide changes" : "Show changes"} onClick={() => showChanges.value = !showChanges.value} disabled={!state.value!.changes.length || saving} />
        <DropdownItem text="Discard changes" onClick={discard} disabled={!state.value!.changes.length || saving} />
        <DropdownSeparator text="Browse" />
        {editRoutes.length === 1 && <DropdownItem text="🌎 View" href={editRoutes[0]} />}
        {editRoutes.length > 1 && <>
          {editRoutes.map(url => <DropdownItem text={`🌎 ${url}`} href={url} />)}
        </>}
      </Dropdown>
    </div>
    {ValidationManager.anyIsInvalid(state.value!.validationResults) &&
      <div class="alert alert-warning mt-3" role="alert">
        The form contains validation errors that need to be reviewed before proceeding.
      </div>}
  </div>
};

export default EditHeader;