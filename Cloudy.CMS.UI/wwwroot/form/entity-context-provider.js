import { useState, useEffect, html } from '../preact-htm/standalone.module.js';
import EntityContext from "./entity-context.js";
import stateManager from '../data/state-manager.js';
import changeManager from '../data/change-manager.js';
import stateEvents from '../data/state-events.js';

export default ({ entityType, keyValues, children }) => {
  const [entityReference, setEntityReference] = useState();
  const [state, setState] = useState();

  useEffect(() => {
    let entityReference;

    if (keyValues) {
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

      if(!state) {
        state = stateManager.createStateForNewEntity(entityType);
        
        searchParams.set("newEntityKey", state.entityReference.newEntityKey);
        history.replaceState({}, null, `${location.pathname}?${searchParams}`);
      }

      entityReference = state.entityReference;
      setEntityReference(entityReference);
      setState(state);
    }

    const callback = () => setState({ ...stateManager.getState(entityReference) });
    stateEvents.onStateChange(entityReference, callback);
    return () => stateEvents.offStateChange(entityReference, callback);
  }, [keyValues]);

  return html`<${EntityContext.Provider} value=${{ entityReference, state }}>
    ${entityReference && state && !state.loading && children}
  <//>`;
};