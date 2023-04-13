import MainMenu from "../layout/main-menu";
import Navbar from "../layout/navbar";
import { useContext } from "preact/hooks";
import EntityTypesContext from "../form/contexts/entity-types-context";
import Table from "../entity-list/table";

const ListView = ({ entityTypeName }) => {
  const { entityTypes } = useContext(EntityTypesContext);
  return <div class="layout">
    <MainMenu />
    <Navbar title={entityTypes[entityTypeName] && entityTypes[entityTypeName].pluralName} />
    <div className="layout-main-panel">
      <Table entityType={entityTypeName} expanded={true} />
    </div>
  </div>
};

export default ListView;