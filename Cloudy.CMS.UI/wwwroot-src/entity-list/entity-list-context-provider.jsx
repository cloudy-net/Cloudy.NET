import { useState } from "preact/hooks";
import EntityListContext from "./entity-list-context.js";

export default ({ children }) => {
  const [lists, setLists] = useState({});

  const getOrUpdateList = (entityTypeName, generateFunction) => {
    const list = lists[entityTypeName];

    if (list) {
      return list.data;
    }

    return setLists({ ...lists, [entityTypeName]: { data: generateFunction() } });
  };
  return <EntityListContext.Provider value={{ getOrUpdateList }}>
    {children}
  </EntityListContext.Provider>;
};