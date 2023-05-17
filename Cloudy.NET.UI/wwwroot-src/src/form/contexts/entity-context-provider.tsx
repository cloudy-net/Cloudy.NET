import { useEffect } from 'preact/hooks';
import EntityContext from "./entity-context";
import stateManager from '../../data/state-manager';
import stateEvents from '../../data/state-events';
import State from "../../data/state"
import EntityReference from '../../data/entity-reference';
import { ComponentChildren } from 'preact';
import StateChangeCallback from '../../data/state-change-callback';
import { useSignal } from '@preact/signals';

export default ({ entityType, keyValues, children }: { entityType: string, keyValues: string[], children: ComponentChildren }) => {
  const entityReference = useSignal<EntityReference | null>(null);
  const state = useSignal<State | null>(null);
  
  useEffect(() => {
    let newEntityReference: EntityReference;

    if (keyValues && keyValues.length) {
      newEntityReference = { entityType, keyValues };
      entityReference.value = newEntityReference;
      const newState = stateManager.createOrUpdateStateForExistingEntity(newEntityReference);
      state.value = newState;
    } else {
      const searchParams = new URLSearchParams(window.location.search);
      const newEntityKey = searchParams.get('newEntityKey');
      const newEntityType = searchParams.get('EntityType')
      
      if(!newEntityType){
        return;
      }
      if(!newEntityKey){
        return;
      }

      let newState = stateManager.getState({
        entityType: newEntityType,
        newEntityKey, // may be null, resulting in null state
      });

      // if state doesn't exist, either because the new entity key has already been
      // saved into a real, existing entity, or that the key is missing from the query
      // string, we create a new state with accompanying new entity key

      if (!newState) {
        newState = stateManager.createStateForNewEntity(entityType);

        searchParams.set("newEntityKey", newState.entityReference.newEntityKey!);
        history.replaceState({}, "", `${location.pathname}?${searchParams}`);
      }

      newEntityReference = newState.entityReference;
      entityReference.value = newEntityReference;
      state.value = newState;
    }

    const stateChange: StateChangeCallback = newState => state.value = newState ? { ...newState } : null;
    stateEvents.onStateChange(stateChange);
    const entityReferenceChange = (newEntityReference: EntityReference) => {
      const searchParams = new URLSearchParams(window.location.search);
      searchParams.delete("newEntityKey");
      searchParams.delete("keys");
      if (newEntityReference.keyValues) {
        for (let key of newEntityReference.keyValues) {
          searchParams.append("keys", key);
        }
      }
      history.replaceState({}, "", `${window.location.pathname}?${searchParams}`);
      entityReference.value = newEntityReference;
    };
    stateEvents.onEntityReferenceChange(entityReferenceChange);
    return () => {
      stateEvents.offStateChange(stateChange);
      stateEvents.offEntityReferenceChange(entityReferenceChange);
    };
  }, [keyValues]);

  return <EntityContext.Provider value={{ entityReference, state }}>
    {entityReference.value && state.value && !state.value.loading && children}
  </EntityContext.Provider>;
};