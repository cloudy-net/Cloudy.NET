import { useState, useEffect, html } from '../preact-htm/standalone.module.js';
import EntityContext from "./entity-context.js";
import stateManager from '../data/state-manager.js';
import changeManager from '../data/change-manager.js';
import stateEvents from '../data/state-events.js';

export default ({ entityType, keyValues, children }) => {
  const [entityReference, setEntityReference] = useState();
  const [state, setState] = useState();
  const [sourceConflicts, setSourceConflicts] = useState([]);
  const [mergedChanges, setMergedChanges] = useState([]);

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
      setMergedChanges([]);
      return;
    }

    setMergedChanges(changeManager.getMergedChanges(state));
  }, [state]);

  useEffect(() => {
    if (!state) {
      setSourceConflicts([]);
      return;
    }

    if (!mergedChanges.length) {
      setSourceConflicts([]);
      return;
    }

    setSourceConflicts(changeManager.getSourceConflicts(state, mergedChanges));
  }, [state, mergedChanges]);

  const clearSourceConflicts = () => setSourceConflicts([]);
  const clearMergedChanges = () => setMergedChanges([]);

  return html`<${EntityContext.Provider} value=${{ entityReference, state, mergedChanges, sourceConflicts, clearSourceConflicts, clearMergedChanges }}>
    ${entityReference && state && !state.loading && children || 'Loading ...'}
  <//>`;
};