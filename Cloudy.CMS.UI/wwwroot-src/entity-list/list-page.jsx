import Table from './table';

const ListPage = ({ entityTypeName }) => {
  return <Table entityType={entityTypeName} expanded={true} />;
}

export default ListPage;