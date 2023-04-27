import { createContext } from 'preact';
import EntityType from './entity-type';

const EntityTypesContext = createContext({} as {
  entityTypes: { [key: string]: EntityType },
  groupedEntityTypes: { [key: string]: EntityType[] },
 });

export default EntityTypesContext;