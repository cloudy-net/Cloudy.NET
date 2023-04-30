import { useState, useEffect } from 'preact/hooks';
import EntityContext from "./entity-context.js";
import stateManager from '../../data/state-manager.js';
import stateEvents from '../../data/state-events.js';
import State from "../../data/state"
import EntityReference from '../../data/entity-reference';
import { ComponentChildren } from 'preact';

export default ({ entityType, keyValues, children }: { entityType: string, keyValues: string[], children: ComponentChildren }) => {
  const [entityReference, setEntityReference] = useState<EntityReference>();
  const [state, setState] = useState<State>();

  useEffect(() => {
    let entityReference;

    if (keyValues && keyValues.length) {
      entityReference = { entityType, keyValues };
      setEntityReference(entityReference);
      const state = stateManager.createOrUpdateStateForExistingEntity(entityReference);
      setState(state);
    } else {
      const searchParams = new URLSearchParams(window.location.search);
      const newEntityKey = searchParams.get('newEntityKey');

      let state = stateManager.getState({
        entityType: searchParams.get('EntityType'),
        newEntityKey, // may be null, resulting in null state
      });

      // if state doesn't exist, either because the new entity key has already been
      // saved into a real, existing entity, or that the key is missing from the query
      // string, we create a new state with accompanying new entity key

      if (!state) {
        state = stateManager.createStateForNewEntity(entityType);

        searchParams.set("newEntityKey", state.entityReference.newEntityKey);
        history.replaceState({}, "", `${location.pathname}?${searchParams}`);
      }

      entityReference = state.entityReference;
      setEntityReference(entityReference);
      setState(state);
    }

    const stateChange = (state: State) => setState({ ...state });
    stateEvents.onStateChange(stateChange);
    const entityReferenceChange = (entityReference: EntityReference) => {
      const searchParams = new URLSearchParams(window.location.search);
      searchParams.delete("newEntityKey");
      searchParams.delete("keys");
      for (let key of entityReference.keyValues) {
        searchParams.append("keys", key);
      }
      history.replaceState({}, "", `${window.location.pathname}?${searchParams}`);
      setEntityReference(entityReference);
    };
    stateEvents.onEntityReferenceChange(entityReferenceChange);
    return () => {
      stateEvents.offStateChange(stateChange);
      stateEvents.offEntityReferenceChange(entityReferenceChange);
    };
  }, [keyValues]);

  return <EntityContext.Provider value={{ entityReference, state }}>
    {entityReference && state && !state.loading && children}
  </EntityContext.Provider>;
};