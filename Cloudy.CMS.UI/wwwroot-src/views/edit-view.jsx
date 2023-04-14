import MainMenu from "../layout/main-menu";
import Navbar from "../layout/navbar";
import Form from "../form/form"
import CompactList from "../entity-list/compact-list";

const EditView = ({ entityTypeName, keyValues }) => <div class="layout show-navigation-panel">
  <MainMenu />
  <Navbar title="Edit" />
  <CompactList entityType={entityTypeName} keyValues={keyValues} />
  <div className="layout-main-panel">
    <Form key={'form-edit'} mode="edit" entityTypeName={entityTypeName} keyValues={keyValues} />
  </div>
</div>;

export default EditView;