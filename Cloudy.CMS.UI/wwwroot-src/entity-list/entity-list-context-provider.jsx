import { useEffect, useState } from "preact/hooks";
import EntityListContext from "./entity-list-context.js";

export default ({ children }) => {
  const [settings, setSettings] = useState({ $loading: true });
  const [components, setComponents] = useState({ $loading: true });
  const [results, setResults] = useState({ data: {} });

  useEffect(function () {
    (async () => {
      const response = await fetch(`/Admin/api/list/settings`, { credentials: "include" });
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
        ? url.startsWith("/") ? window.location.origin : "./"
        : url.startsWith("/") ? "" : "./";

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

  const getResult = entityType => {
    let result = results[entityType];

    if(!result){
      result = results[entityType] = {
        $loading: true,
        data: null,
        page: 1,
        pages: [],
        filters: [],
        search: "",
        orderBy: "",
        orderByDirection: "asc",
      }
    }

    return result;
  };

  const loadResult = async (entityType, page, filters, search, orderBy, orderByDirection) => {
    const filterComponent = Object.entries(filters).map(([key, value]) => `filters[${key}]=${encodeURIComponent(Array.isArray(value) ? JSON.stringify(value) : value)}`).join("&");
    const url = `/Admin/api/list/result?entityType=${entityType}&columns=${settings[entityType].columns.map(c => c.name).join(",")}&${filterComponent}&pageSize=${settings[entityType].pageSize}&page=${page}&search=${search}&orderBy=${orderBy}&orderByDirection=${orderByDirection}`;

    if(results[entityType].url == url){
      return results[entityType];
    }

    const response = await fetch(url, { credentials: "include" });
    const body = response.ok ? await response.json() : await response.text();
    const pageCount = response.ok ? Math.ceil(body.totalCount / settings[entityType].pageSize) : 0;

    setResults({
      ...results,
      [entityType]: {
        $loading: false,
        url,
        data: response.ok && body,
        error: !response.ok && { response, body },
        page,
        pageCount,
        pages: [...Array(pageCount)],
        filters,
        search,
        orderBy,
        orderByDirection,
      }
    })
  }

  return <EntityListContext.Provider value={{ settings, components, results, getResult, loadResult }}>
    {children}
  </EntityListContext.Provider>;
};