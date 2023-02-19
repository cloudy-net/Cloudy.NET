import { html, useState, useEffect } from '../preact-htm/standalone.module.js';
import FieldComponentContext from "./field-component-context.js";

export default ({ children }) => {
  const [components, setComponents] = useState();

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

      const componentPromises = urls.map(url => ({ url, promise: import(url) }));

      await Promise.allSettled(componentPromises.map(c => c.promise));

      const result = {};

      for(let c of componentPromises){
        result[c.url] = (await c.promise).default;
      }

      setComponents(result);
    })();
  }, []);

  return html`<${FieldComponentContext.Provider} value=${components}>
    ${children}
  <//>`;
};