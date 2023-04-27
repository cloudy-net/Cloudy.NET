import DashboardPanel from "../layout/dashboard-panel";
import MainMenu from "../layout/main-menu";
import Navbar from "../layout/navbar";

const DashboardView = () => <div class="layout">
  <MainMenu />
  <Navbar title="Dashboard" />
  <div className="layout-main-panel">
    <DashboardPanel />
  </div>
</div>;

export default DashboardView;