
import { useState, useEffect, useContext } from 'preact/hooks';
import EntityContext from '@src/form/contexts/entity-context';
import simpleChangeHandler from '@src/data/change-handlers/simple-change-handler.js';
import { ReactComponent as Caret } from "../assets/caret-horizontal.svg";

const EditHeader = ({ entityTypeName, keyValues }) => {
  const [instanceName, setInstanceName] = useState();
  const [editRoutes, setEditRoutes] = useState([]);
  const { entityReference, state } = useContext(EntityContext);

  useEffect(() => setInstanceName(simpleChangeHandler.getIntermediateValue(state, 'Name')), [entityReference]);

  useEffect(() => {
    (async () => await fetch(
      `/Admin/api/layout/content-routes?entityTypeName=${entityTypeName}&keys=${keyValues.join('&keys=')}`,
      { credentials: 'include' })
      .then(r => r.json())
      .then(r => setEditRoutes(r))
    )();
  }, []);

  return <div className="form-header">
    <div className="form-breadcrumb">
      <a className="form-breadcrumb-item" href={`/Admin`}>Dashboard <Caret className="form-breadcrumb-caret" /></a>
      <a className="form-breadcrumb-item" href={`/Admin/List/${entityTypeName}`}>{entityTypeName} <Caret className="form-breadcrumb-caret" /></a>
      <a className="form-breadcrumb-item">{instanceName} <Caret className="form-breadcrumb-caret" /></a>
      <a className="form-breadcrumb-item active">Edit</a>
    </div>
    {instanceName}&nbsp;
    <a class="btn btn-sm btn-primary" href={`/Admin/New/${entityTypeName}`}>New</a>&nbsp;
    {editRoutes.length === 1 && <a class="btn btn-beta btn-sm" href={editRoutes[0]}>View</a>}
    {editRoutes.length > 1 && <div class="dropdown d-inline-block">
      <button class="btn btn-beta btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
        View
      </button>
      <ul class="dropdown-menu">
        {editRoutes.map(url => <li><a class="dropdown-item" href={url}>🌎 {url}</a></li>)}
      </ul>
    </div>}
  </div>
};

export default EditHeader;