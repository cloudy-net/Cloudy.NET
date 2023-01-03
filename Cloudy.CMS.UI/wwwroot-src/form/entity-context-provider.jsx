import { useState, useEffect } from '@preact-htm';
import EntityContext from "./entity-context";

export default ({ contentType, keys, children }) => {
  return <EntityContext.Provider value={{ contentType, keys }}>
    {children}
  </EntityContext.Provider>;
};