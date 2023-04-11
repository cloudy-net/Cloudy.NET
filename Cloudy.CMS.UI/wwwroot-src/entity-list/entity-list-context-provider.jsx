import { useEffect, useState } from "preact/hooks";
import EntityListContext from "./entity-list-context.js";

export default ({ children }) => {
  const [settings, setSettings] = useState({ $loading: true });
  const [components, setComponents] = useState({ $loading: true });

  useEffect(function () {
    (async () => {
      const response = await fetch(`/Admin/api/list/settings`, { credentials: 'include' });
      const json = await response.json();
      setSettings(json);
    })();
  }, []);

  useEffect(() => {
    (async () => {
      if (settings.$loading) {
        return;
      }

      const getUrlPrefix = (url) => /* @vite-ignore */ window.viteDevServerIsRunning
        ? url.startsWith('/') ? window.location.origin : './'
        : url.startsWith('/') ? '' : './';

      const componentPartials = [... new Set(Object.values(settings).flatMap(s => s.columns.map(c => c.partial)))];

      const componentPromises = componentPartials.map(url => ({ url, promise: import(`${getUrlPrefix(url)}${url}`) }));

      await Promise.allSettled(componentPromises.map(c => c.promise));

      const result = {};

      for (let c of componentPromises) {
        result[c.url] = (await c.promise).default;
      }

      setComponents(result);
    })();
  }, [settings]);

  return <EntityListContext.Provider value={{ settings, components }}>
    {children}
  </EntityListContext.Provider>;
};