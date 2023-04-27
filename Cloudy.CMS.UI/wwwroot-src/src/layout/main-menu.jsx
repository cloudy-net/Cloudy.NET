import { useContext, useState } from "preact/hooks";
import EntityTypesContext from "../form/contexts/entity-types-context";
import { ReactComponent as DashboardIcon } from "../assets/icon-dashboard.svg";
import { ReactComponent as EntityTypeIcon } from "../assets/icon-entity-type.svg";
import { ReactComponent as Cloud } from "../assets/cloud.svg";
import { ReactComponent as Caret } from "../assets/caret-vertical.svg";
import { Link } from "preact-router/match";

const MainMenu = () => {
  const { groupedEntityTypes } = useContext(EntityTypesContext);
  const [expanded, setExpanded] = useState({});

  return <div className="main-menu">
    <Cloud className="main-menu-cloud" />
    <ul className="main-menu-items">
      <li className="main-menu-item">
        <Link activeClassName="active" className="main-menu-item-button" href="/Admin"><DashboardIcon className="main-menu-item-icon" /> Dashboard</Link>
      </li>
      {Object.keys(groupedEntityTypes).sort().map(key => <>
        <li className="main-menu-item">
          <a className="main-menu-item-button" tabIndex="0" onClick={() => setExpanded({...expanded, [key]: !expanded[key]})}><EntityTypeIcon className="main-menu-item-icon" /> {key} <Caret className={"main-menu-item-caret" + (expanded[key] ? " expanded" : "")} /></a>
          {expanded[key] && <ul class="main-menu-sub-items">
            {groupedEntityTypes[key].map(entityType =>
              <li className="main-menu-sub-item">
                <Link activeClassName="active" class="main-menu-sub-item-button" href={`/Admin/List/${entityType.entityTypeName}`}>{entityType.pluralName}</Link>
              </li>
            )}
          </ul>}
        </li>
      </>)}
    </ul>
  </div>;
}

export default MainMenu;