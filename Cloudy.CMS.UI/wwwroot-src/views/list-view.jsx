import MainMenu from "../layout/main-menu";
import Navbar from "../layout/navbar";
import ListPage from "../entity-list/list-page";
import { useContext } from "preact/hooks";
import EntityTypesContext from "../form/contexts/entity-types-context";

const ListView = ({ entityTypeName }) => {
  const { entityTypes } = useContext(EntityTypesContext);
  return <div class="layout">
    <MainMenu />
    <Navbar title={ entityTypes[entityTypeName] && entityTypes[entityTypeName].pluralName } />
    <div className="layout-main-panel">
      <ListPage entityTypeName={entityTypeName} />
    </div>
  </div>
};

export default ListView;