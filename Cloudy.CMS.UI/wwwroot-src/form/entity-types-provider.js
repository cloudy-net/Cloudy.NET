import { useState, useEffect } from 'preact/hooks';
import EntityTypesContext from './entity-types-context';

export default ({ children }) => {
  const [entityTypes, setEntityTypes] = useState([]);

  useEffect(function () {
    (async () => {
      const response = await fetch('/Admin/api/entity-type-list/result', { credentials: 'include' });

      if (!response.ok) {
        throw { response, body: await response.text() };
      }

      setEntityTypes(await response.json());
    })();
  }, []);

  return <EntityTypesContext.Provider value={{entityTypes}}>
    {children}
  </EntityTypesContext.Provider>;
};