import { useContext } from "preact/hooks";
import EntityTypesContext from "../form/contexts/entity-types-context";
import { ReactComponent as DashboardIcon } from "../assets/icon-dashboard.svg";

const MainMenu = () => {
  const { groupedEntityTypes } = useContext(EntityTypesContext);

  return <div className="main-menu">
    <div className="main-menu-cloud">☁</div>
    <ul className="main-menu-items">
      <li className="main-menu-item">
        <a className="main-menu-item-button active" tabIndex="0"><DashboardIcon className="main-menu-item-icon" /> Dashboard</a>
      </li>
      {Object.keys(groupedEntityTypes).sort().map(key => <>
        <li className="main-menu-item">
          <a className="main-menu-item-button" tabIndex="0">{key}</a>
          <ul class="main-menu-sub-items">
            {groupedEntityTypes[key].map(entityType =>
              <li className="main-menu-sub-item">
                <a className="main-menu-sub-item-button" tabIndex="0">{entityType.pluralName}</a>
              </li>
            )}
          </ul>
        </li>
      </>)}
    </ul>
  </div>;
}

export default MainMenu;