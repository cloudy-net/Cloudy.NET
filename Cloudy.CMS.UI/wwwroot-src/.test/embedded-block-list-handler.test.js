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
          [propertyName]: [{ Type: blockType }, { Type: blockType }]
        }
      }
    });

    assert.deepEqual(embeddedBlockListHandler.getIntermediateValue(stateManager.getState(entityReference), propertyName), [{ key: '0', type: blockType }, { key: '1', type: blockType }]);
    const item = embeddedBlockListHandler.add(entityReference, propertyName, blockType);
    assert.deepEqual(embeddedBlockListHandler.getIntermediateValue(stateManager.getState(entityReference), propertyName), [{ key: '0', type: blockType }, { key: '1', type: blockType }, { key: item.key, type: blockType }]);
  });
  it('should not merge change', () => {
    global.localStorage.clear();
    stateManager.states = statePersister.loadStates();
    const propertyName = 'TestProperty';
    const blockType = 'BlockType';
    const state = stateManager.createStateForNewEntity('page');
    state.history = [
      { '$type': 'embeddedblocklist.add', date: Date.now(), path: propertyName, key: 'lorem', type: blockType },
    ];

    embeddedBlockListHandler.add(state.entityReference, propertyName, blockType);

    assert.equal(state.history.length, 2);
  });
  it('remove new element', () => {
    global.localStorage.clear();
    stateManager.states = statePersister.loadStates();
    const { entityReference } = stateManager.createStateForNewEntity('page');
    const propertyName = 'TestProperty';
    const blockType = 'BlockType';

    const item = embeddedBlockListHandler.add(entityReference, propertyName, blockType);
    assert.deepEqual(embeddedBlockListHandler.getIntermediateValue(stateManager.getState(entityReference), propertyName), [{ key: item.key, type: item.type }]);
    embeddedBlockListHandler.remove(entityReference, propertyName, item.key, item.type);
    assert.deepEqual(embeddedBlockListHandler.getIntermediateValue(stateManager.getState(entityReference), propertyName), []);
  });
});