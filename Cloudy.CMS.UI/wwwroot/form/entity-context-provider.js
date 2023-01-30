import { useState, useEffect, html } from '../preact-htm/standalone.module.js';
import EntityContext from "./entity-context.js";
import stateManager from '../data/state-manager.js';

export default ({ entityType, keyValues, children }) => {
  const [entityReference, setEntityReference] = useState();
  const [state, setState] = useState();

  useEffect(() => {
    let entityReference;

    if (keyValues) {
      entityReference = { entityType, keyValues };
      setEntityReference(entityReference);
      const state = stateManager.createOrUpdateStateForExistingContent(entityReference);
      setState(state);
    } else {
      const state = stateManager.createStateForNewContent(entityType);
      entityReference = state.entityReference
      setEntityReference(entityReference);
      setState(state);
    }

    const callback = () => setState({...stateManager.getState(entityReference)});
    stateManager.onStateChange(entityReference, callback);
    return () => stateManager.offStateChange(entityReference, callback);
  }, [keyValues]);

  return html`<${EntityContext.Provider} value=${{ entityReference, state, mergedChanges: state && stateManager.getMergedChanges(state) }}>
    ${entityReference && state && !state.loading && children || 'Loading ...'}
  <//>`;
};