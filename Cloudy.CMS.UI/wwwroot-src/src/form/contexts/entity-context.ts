import { createContext } from 'preact';
import EntityReference from '../../data/entity-reference';
import State from '../../data/state';

const EntityContext = createContext<{ entityReference: EntityReference | null, state: State | null } | null>(null);

export default EntityContext;