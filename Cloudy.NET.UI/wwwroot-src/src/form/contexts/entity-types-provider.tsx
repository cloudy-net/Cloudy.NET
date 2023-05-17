import { useState, useEffect } from 'preact/hooks';
import groupby from '../../util/groupby';
import EntityTypesContext from './entity-types-context';
import { ComponentChildren } from 'preact';
import EntityType from './entity-type';

export default ({ children }: { children: ComponentChildren }) => {
  const [entityTypes, setEntityTypes] = useState({});
  const [groupedEntityTypes, setGroupedEntityTypes] = useState({} as { [key: string]: EntityType[] });

  useEffect(function () {
    (async () => {
      const response = await fetch('/Admin/api/entity-type-list/result', { credentials: 'include' });

      if (!response.ok) {
        throw { response, body: await response.text() };
      }
      
      const json = (await response.json()) as [EntityType];

      const result: { [key: string]: EntityType } = {};

      for (let type of json) {
        result[type.name] = type;
      }

      setEntityTypes(result);
      setGroupedEntityTypes(groupby(json, 'groupName'));
    })();
  }, []);

  return <EntityTypesContext.Provider value={{ entityTypes, groupedEntityTypes }}>
    {children}
  </EntityTypesContext.Provider>;
};