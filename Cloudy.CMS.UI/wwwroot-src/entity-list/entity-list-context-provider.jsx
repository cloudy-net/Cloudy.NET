import { useEffect, useState } from "preact/hooks";
import EntityListContext from "./entity-list-context.js";

export default ({ children }) => {
  const [settings, setSettings] = useState({ loading: true });

  useEffect(function () {
    (async () => {
      const response = await fetch(`/Admin/api/list/settings`, { credentials: 'include' });
      const json = await response.json();
      setSettings(json);
    })();
  }, []);
  
  return <EntityListContext.Provider value={{ settings }}>
    {children}
  </EntityListContext.Provider>;
};