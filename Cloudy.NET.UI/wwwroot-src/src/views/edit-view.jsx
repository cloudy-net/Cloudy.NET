import MainMenu from "../layout/main-menu";
import Navbar from "../layout/navbar";
import Form from "../form/form"
import CompactList from "../entity-list/compact-list";
import { useContext } from "preact/hooks";
import EntityTypesContext from "../form/contexts/entity-types-context";

const EditView = ({ entityTypeName, mode, keyValues }) => {
  const { entityTypes } = useContext(EntityTypesContext);
  return <div class="layout show-navigation-panel">
    <MainMenu />
    <Navbar title={entityTypes[entityTypeName] && entityTypes[entityTypeName].pluralName} entityTypeName={entityTypeName} />
    <CompactList entityType={entityTypeName} keyValues={keyValues} />
    <div className="layout-main-panel">
      <Form key={'form-edit'} mode={mode} entityTypeName={entityTypeName} keyValues={keyValues} />
    </div>
  </div>;
}

export default EditView;