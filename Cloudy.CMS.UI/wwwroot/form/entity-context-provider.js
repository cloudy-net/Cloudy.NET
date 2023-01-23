import { useState, useEffect, html } from '../preact-htm/standalone.module.js';
import EntityContext from "./entity-context.js";
import stateManager from '../data/state-manager.js';

export default ({ entityType, keyValues, children }) => {
  const [contentReference, setContentReference] = useState();
  const [state, setState] = useState();

  useEffect(() => {
    let contentReference;

    if (keyValues) {
      contentReference = { entityType, keyValues };
      setContentReference(contentReference);
      const state = stateManager.createOrUpdateStateForExistingContent(contentReference);
      setState(state);
    } else {
      const state = stateManager.createStateForNewContent(entityType);
      contentReference = state.contentReference
      setContentReference(contentReference);
      setState(state);
    }

    const callback = () => setState({...stateManager.getState(contentReference)});
    stateManager.onStateChange(contentReference, callback);
    return () => stateManager.offStateChange(contentReference, callback);
  }, [keyValues]);

  return html`<${EntityContext.Provider} value=${{ contentReference, state }}>
    ${contentReference && state && !state.loading && children || 'Loading ...'}
  <//>`;
};