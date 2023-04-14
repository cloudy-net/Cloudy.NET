import { useEffect, useState } from "preact/hooks";
import { ReactComponent as NotificationIcon } from "../assets/icon-notification.svg";
import Dropdown from "../components/dropdown";
import DropdownItem from "../components/dropdown-item";
import DropdownSeparator from "../components/dropdown-separator";
import { ReactComponent as HelpIcon } from "../assets/icon-help.svg";
import { ReactComponent as ChatIcon } from "../assets/icon-chat.svg";
import { ReactComponent as LogoutIcon } from "../assets/icon-logout.svg";



const Navbar = ({ title }) => {
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
      { title || "Dashboard" }
    </div>
    {!settings.isValidLicense ||1 &&
      <div class="navbar-license-nag">
        Unlicensed version.<br/><a className="text-link" href="https://www.cloudy.net/" target="_blank">Click here</a> to purchase a license.
      </div>
    }
    <Dropdown className="button-reset navbar-notification-button" contents={<NotificationIcon className="navbar-notification-icon" />}>
      This is the dropdown
    </Dropdown>
    <Dropdown className="button-reset navbar-profile-button" contents={<>Alfred Pennyworth <span className="navbar-profile-picture"></span></>}>
      <DropdownItem href="https://www.cloudy.net/resources/docs" target="_blank" icon={<HelpIcon/>} text="Tutorials and FAQ" />
      <DropdownItem href="https://github.com/cloudy-net/Cloudy.CMS/issues/new/choose" target="_blank" icon={<ChatIcon/>} text="Support" />
      <DropdownSeparator />
      <DropdownItem href="/Admin/Logout" icon={<LogoutIcon/>} text="Logout" />
    </Dropdown>
  </div>
}

export default Navbar;