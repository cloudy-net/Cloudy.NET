import { useState, useEffect } from 'preact/hooks';
import groupby from '@src/util/groupby';
import EntityTypesContext from './entity-types-context';

export default ({ children }) => {
  const [entityTypes, setEntityTypes] = useState([]);
  const [groupedEntityTypes, setGroupedEntityTypes] = useState([]);

  useEffect(function () {
    (async () => {
      const response = await fetch('/Admin/api/entity-type-list/result', { credentials: 'include' });

      if (!response.ok) {
        throw { response, body: await response.text() };
      }

      const json = await response.json();

      setEntityTypes(json);
      setGroupedEntityTypes(groupby(json, 'groupName'));
    })();
  }, []);

  return <EntityTypesContext.Provider value={{ entityTypes, groupedEntityTypes }}>
    {children}
  </EntityTypesContext.Provider>;
};