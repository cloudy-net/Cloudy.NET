import { useState, useEffect } from '@preact-htm';
import EntityContext from "./entity-context";
import stateManager from '../data/state-manager.js';

export default ({ contentType, keyValues, children }) => {
  const [contentReference, setContentReference] = useState();
  const [state, setState] = useState();

  useEffect(() => {
    let contentReference;

    if (keyValues) {
      contentReference = { contentType, keyValues };
      setContentReference(contentReference);
      stateManager.createOrUpdateStateForExistingContent(contentReference);
    } else {
      const state = stateManager.createStateForNewContent(contentType);
      contentReference = state.contentReference
      setContentReference(contentReference);
    }

    const callback = () => setState(stateManager.getState(contentReference));
    stateManager.onStateChange(contentReference, callback);
    return () => stateManager.offStateChange(contentReference, callback);
  }, [keyValues]);

  return <EntityContext.Provider value={{ contentReference }}>
    {contentReference && state && !state.loading && children || <>Loading ...</>}
  </EntityContext.Provider>;
};