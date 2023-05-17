import { useState, useEffect } from 'preact/hooks';
import FieldComponentContext from "./field-component-context.js";
import { ComponentChildren } from 'preact';

export default ({ children }: { children: ComponentChildren }) => {
  const [components, setComponents] = useState<{ [key: string]: any } | null>(null);

  useEffect(function () {
    (async () => {
      const response = await fetch(
        `/Admin/api/form/fields/components`,
        {
          credentials: 'include'
        }
      );

      if (!response.ok) {
        throw { response, body: await response.text() };
      }

      var urls = await response.json();
      const getUrlPrefix = (url: string) => /* @vite-ignore */ window.viteDevServerIsRunning
        ? url.startsWith('/') ? window.location.origin : '../../'
        : url.startsWith('/') ? '' : './';

      const componentPromises = urls.map((url: string) => ({ url, promise: import(/* @vite-ignore */ `${getUrlPrefix(url)}${url}`) }));

      await Promise.allSettled(componentPromises.map((c: any) => c.promise));

      const result: { [key: string]: any } = {};

      for (let c of componentPromises) {
        result[c.url] = (await c.promise).default;
      }

      setComponents(result);
    })();
  }, []);

  return <FieldComponentContext.Provider value={components}>
    {children}
  </FieldComponentContext.Provider>;
};