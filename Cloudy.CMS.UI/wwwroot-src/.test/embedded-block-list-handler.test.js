import assert from 'assert';
import { } from './polyfiller.js';
import stateManager from '../data/state-manager.js';
import statePersister from '../data/state-persister.js';
import embeddedBlockListHandler from '../data/change-handlers/embedded-block-list-handler.js';

describe('embedded-block-list-handler.js', () => {
  it('intermediate value', () => {
    global.localStorage.clear();
    stateManager.states = statePersister.loadStates();
    const { entityReference } = stateManager.createStateForNewEntity('page');
    const propertyName = 'TestProperty';

    stateManager.replace({
      ...stateManager.getState(entityReference),
      source: {
        value: {
          [propertyName]: [{}, {}]
        }
      }
    });

    assert.deepEqual(embeddedBlockListHandler.getIntermediateValue(stateManager.getState(entityReference), propertyName), ['0', '1']);
    const item = embeddedBlockListHandler.add(entityReference, propertyName);
    assert.deepEqual(embeddedBlockListHandler.getIntermediateValue(stateManager.getState(entityReference), propertyName), ['0', '1', item.key]);
  });
});