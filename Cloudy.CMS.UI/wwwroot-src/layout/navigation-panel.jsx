import Router from 'preact-router';
import Table from '../entity-list/table';

export default () => {
  return <div class="layout-navigation-panel">
    <Router>
      <Table path="/Admin/List/:entityType" expanded={false} />
      <Table path="/Admin/Edit/:entityType" expanded={false} />
      <Table path="/Admin/New/:entityType" expanded={false} />
      <Table path="/Admin/Delete/:entityType" expanded={false} />
    </Router>
  </div>
}