import Router from 'preact-router';
import Table from '../entity-list/table';

export default () => {
  return <div class="layout-navigation-panel">
    <Router>
      <Table path="/Admin/List/:entityType" />
      <Table path="/Admin/Edit/:entityType" />
      <Table path="/Admin/New/:entityType" />
      <Table path="/Admin/Delete/:entityType" />
    </Router>
  </div>
}