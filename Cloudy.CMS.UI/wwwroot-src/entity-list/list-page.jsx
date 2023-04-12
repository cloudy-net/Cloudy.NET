import Table from './table';

const ListPage = ({ entityTypeName }) => {
  return <div class="layout-navigation-panel">
    <Table entityType={entityTypeName} expanded={true} />
  </div>
}

export default ListPage;