import { useContext, useEffect, useState } from "preact/hooks";
import EntityTypesContext from "../form/contexts/entity-types-context";
import { ReactComponent as NotificationIcon } from "../assets/icon-notification.svg";

const Navbar = ({ }) => {
  const { entityTypes } = useContext(EntityTypesContext);

  const [settings, setSettings] = useState(null);

  useEffect(function () {
    (async () => {
      await fetch('/Admin/api/layout/footer', { credentials: 'include' })
        .then(r => r.json())
        .then(json => setSettings(json));
    })();
  }, []);

  return settings && <div class="navbar">
    <div class="navbar-title">
      Dashboard
    </div>
    {!settings.isValidLicense ||1 &&
      <div class="navbar-license-nag">
        Unlicensed version.<br/><a className="text-link" href="https://www.cloudy.net/" target="_blank">Click here</a> to purchase a license.
      </div>
    }
    <a className="navbar-notification-button" tabIndex="0"><NotificationIcon className="navbar-notification-icon" /></a>
    <a className="navbar-profile-button" tabIndex="0">Alfred Pennyworth <span className="navbar-profile-picture"></span></a>
  </div>
}

export default Navbar;