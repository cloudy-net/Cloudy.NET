import { createContext } from 'preact';
import EntityReference from '../../data/entity-reference';
import State from '../../data/state';
import { Signal } from "@preact/signals-core";

const EntityContext = createContext<{ entityReference: Signal<EntityReference | null>, state: Signal<State | null> }>({
  
});

export default EntityContext;