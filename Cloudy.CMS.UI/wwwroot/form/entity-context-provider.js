import { useState, useEffect, html } from '../preact-htm/standalone.module.js';
import EntityContext from "./entity-context.js";
import stateManager from '../data/state-manager.js';
import changeManager from '../data/change-manager.js';
import stateEvents from '../data/state-events.js';

export default ({ entityType, keyValues, children }) => {
  const [entityReference, setEntityReference] = useState();
  const [state, setState] = useState();
  const [changes, setChanges] = useState([]);

  useEffect(() => {
    let entityReference;

    if (keyValues) {
      entityReference = { entityType, keyValues };
      setEntityReference(entityReference);
      const state = stateManager.createOrUpdateStateForExistingContent(entityReference);
      setState(state);
    } else {
      const state = stateManager.createStateForNewContent(entityType);
      entityReference = state.entityReference;
      setEntityReference(entityReference);
      setState(state);
    }

    const callback = () => setState({ ...stateManager.getState(entityReference) });
    stateEvents.onStateChange(entityReference, callback);
    return () => stateEvents.offStateChange(entityReference, callback);
  }, [keyValues]);

  useEffect(() => {
    if (!state) {
      setChanges([]);
      return;
    }

    setChanges(changeManager.getChanges(state));
  }, [state]);

  const clearChanges = () => setChanges([]);

  return html`<${EntityContext.Provider} value=${{ entityReference, state, changes, clearChanges }}>
    ${entityReference && state && !state.loading && children || 'Loading ...'}
  <//>`;
};