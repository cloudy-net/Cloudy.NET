import { useContext } from 'preact/hooks';
import EntityTypesContext from '@src/form/entity-types-context';
import Router from 'preact-router';
import Table from '../list-page/table';

export default () => {
  const { entityTypes } = useContext(EntityTypesContext);

  return <div class="layout-left-panel">
    <div class="dropdown">
      <button class="btn btn-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
        Dropdown button
      </button>
      <ul class="dropdown-menu">
        {entityTypes.map(entityType =>
          <li><a class="dropdown-item" href={`/Admin/${entityType.links[0].action}/${entityType.links[0].entityTypeName}`} title={entityType.description}>{entityType.pluralName}</a></li>
        )}
      </ul>
    </div>

    <Router>
      <Table path="/Admin/List/:entityType" />
      <Table path="/Admin/Edit/:entityType" />
      <Table path="/Admin/New/:entityType" />
      <Table path="/Admin/Delete/:entityType" />
    </Router>
  </div>
}