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
    const blockType = 'BlockType';

    stateManager.replace({
      ...stateManager.getState(entityReference),
      source: {
        value: {
          [propertyName]: [{ type: blockType }, { type: blockType }]
        }
      }
    });

    assert.deepEqual(embeddedBlockListHandler.getIntermediateValue(stateManager.getState(entityReference), propertyName), [{ key: '0', type: blockType }, { key: '1', type: blockType }]);
    const item = embeddedBlockListHandler.add(entityReference, propertyName, blockType);
    assert.deepEqual(embeddedBlockListHandler.getIntermediateValue(stateManager.getState(entityReference), propertyName), [{ key: '0', type: blockType }, { key: '1', type: blockType }, { key: item.key, type: blockType }]);
  });
});