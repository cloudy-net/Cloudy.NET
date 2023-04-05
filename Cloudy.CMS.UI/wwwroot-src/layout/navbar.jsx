import { useContext, useEffect, useState } from "preact/hooks";
import EntityTypesContext from "../form/contexts/entity-types-context";

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
      <div class="navbar-license-nag text-links">
        Unlicensed version.<br/><a href="https://www.cloudy.net/" target="_blank">Click here</a> to purchase a license.
      </div>
    }

  </div>
}

export default Navbar;