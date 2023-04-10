import MainMenu from "../layout/main-menu";
import Navbar from "../layout/navbar";
import ListPage from "../list-page/list-page";

const ListView = ({ entityTypeName }) => {
  return <div class="layout">
    <MainMenu />
    <Navbar title="List" />
    <div className="layout-main-panel">
      <ListPage entityTypeName={entityTypeName} />
    </div>
  </div>
};

export default ListView;