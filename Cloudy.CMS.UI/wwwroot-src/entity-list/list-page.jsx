import Table from './table';

const ListPage = ({ entityTypeName }) => {
  return <div class="layout-navigation-panel">
    <Table entityType={entityTypeName} />
  </div>
}

export default ListPage;