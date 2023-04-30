import { createContext } from 'preact';
import EntityReference from '../../data/entity-reference';
import State from '../../data/state';

const EntityContext = createContext<{ entityReference: EntityReference, state: State } | null>(null);

export default EntityContext;