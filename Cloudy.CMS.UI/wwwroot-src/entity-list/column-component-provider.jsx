import { useState, useEffect } from 'preact/hooks';
import ColumnComponentContext from './column-component-context';

export default ({ children, componentPartials }) => {
  const [components, setComponents] = useState();

  useEffect(() => {
    (async () => {
      if (!componentPartials) return;
      const getUrlPrefix = (url) => /* @vite-ignore */ window.viteDevServerIsRunning 
        ? url.startsWith('/') ? window.location.origin : './' 
        : url.startsWith('/') ? '' : './';

      const componentPromises = componentPartials.map(url => ({url, promise: import(`${getUrlPrefix(url)}${url}`)}));

      await Promise.allSettled(componentPromises.map(c => c.promise));

      const result = {};

      for(let c of componentPromises){
        result[c.url] = (await c.promise).default;
      }

      setComponents(result);
    })();
  }, [componentPartials]);

  return <ColumnComponentContext.Provider value={components}>
    {children}
  </ColumnComponentContext.Provider>;
};