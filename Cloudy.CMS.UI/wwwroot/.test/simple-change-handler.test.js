import assert from 'assert';
import { } from './polyfiller.js';
import stateManager from '../data/state-manager.js';
import simpleChangeHandler from '../data/change-handlers/simple-change-handler.js';
import blockTypeChangeHandler from '../data/change-handlers/block-type-change-handler.js';
import statePersister from '../data/state-persister.js';

describe('simple-change-handler.js', () => {
  describe('simple scenario', () => {
    it('intermediate value', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const { entityReference } = stateManager.createStateForNewContent('page');
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';
      const newValue = 'ipsum';

      stateManager.replace({
        ...stateManager.getState(entityReference),
        source: {
          value: {
            [propertyName]: initialValue
          }
        }
      });

      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference), propertyName), initialValue);
      simpleChangeHandler.setValue(entityReference, propertyName, newValue);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference), propertyName), newValue);
    });
    it('change should be deleted if it equals source', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const { entityReference } = stateManager.createStateForNewContent('page');
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';

      stateManager.replace({
        ...stateManager.getState(entityReference),
        source: {
          value: {
            [propertyName]: initialValue
          }
        }
      });

      simpleChangeHandler.setValue(entityReference, propertyName, initialValue);
      assert.equal(stateManager.getState(entityReference).history.length, 0);
    });
    it('change should not be deleted if it equals source but previous change.s exist', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const { entityReference } = stateManager.createStateForNewContent('page');
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';

      stateManager.replace({
        ...stateManager.getState(entityReference),
        source: {
          value: {
            [propertyName]: initialValue
          }
        },
        history: [
          { '$type': 'simple', date: Date.now() - 10 * 60 * 1000, path: propertyName, value: '' },
        ]
      });

      simpleChangeHandler.setValue(entityReference, propertyName, initialValue);
      assert.equal(stateManager.getState(entityReference).history.length, 2);
    });
    it('intermediate value, deep path', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const { entityReference } = stateManager.createStateForNewContent('page');
      const blockName = 'Block1';
      const block2Name = 'Block2';
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';
      const newValue = 'ipsum';

      stateManager.replace({
        ...stateManager.getState(entityReference),
        source: {
          value: {
            [blockName]: {
              Value: {
                [block2Name]: {
                  Value: {
                    [propertyName]: initialValue
                  }
                }
              }
            }
          }
        }
      });

      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference), `${blockName}.${block2Name}.${propertyName}`), initialValue);
      simpleChangeHandler.setValue(entityReference, `${blockName}.${block2Name}.${propertyName}`, newValue);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference), `${blockName}.${block2Name}.${propertyName}`), newValue);
    });
  });
  describe('complex scenario', () => {
    it('simple change after changing block type', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const { entityReference } = stateManager.createStateForNewContent('page');
      const newType = 'BlockType';
      const blockName = 'Block';
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';
      const newValue = 'ipsum';

      stateManager.replace({
        ...stateManager.getState(entityReference),
        source: {
          value: {
            [blockName]: {
              Value: {
                [propertyName]: initialValue
              }
            }
          }
        }
      });

      blockTypeChangeHandler.setType(entityReference, blockName, newType);
      simpleChangeHandler.setValue(entityReference, `${blockName}.${propertyName}`, newValue);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference), `${blockName}.${propertyName}`), newValue);
    });
    it('changing block type after simple change', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const { entityReference } = stateManager.createStateForNewContent('page');
      const newType = 'BlockType';
      const blockName = 'Block1';
      const block2Name = 'Block2';
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';
      const newValue = 'ipsum';

      stateManager.replace({
        ...stateManager.getState(entityReference),
        source: {
          value: {
            [blockName]: {
              Value: {
                [block2Name]: {
                  Value: {
                    [propertyName]: initialValue
                  }
                }
              }
            }
          }
        }
      });

      simpleChangeHandler.setValue(entityReference, `${blockName}.${block2Name}.${propertyName}`, newValue);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference), `${blockName}.${block2Name}.${propertyName}`), newValue);
      blockTypeChangeHandler.setType(entityReference, `${blockName}.${block2Name}`, newType);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference), `${blockName}.${block2Name}.${propertyName}`), null);
      simpleChangeHandler.setValue(entityReference, `${blockName}.${block2Name}.${propertyName}`, newValue);
      assert.equal(stateManager.getState(entityReference).history.length, 3);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference), `${blockName}.${block2Name}.${propertyName}`), newValue);
      blockTypeChangeHandler.setType(entityReference, blockName, newType);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference), `${blockName}.${block2Name}.${propertyName}`), null);
    });
    it('intermediate value should be cleared when changing parents parents block type', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const { entityReference } = stateManager.createStateForNewContent('page');
      const newType = 'BlockType';
      const blockName = 'Block1';
      const block2Name = 'Block2';
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';
      const newValue = 'ipsum';

      stateManager.replace({
        ...stateManager.getState(entityReference),
        source: {
          value: {
            [blockName]: {
              Value: {
                [block2Name]: {
                  Value: {
                    [propertyName]: initialValue
                  }
                }
              }
            }
          }
        }
      });

      simpleChangeHandler.setValue(entityReference, `${blockName}.${block2Name}.${propertyName}`, newValue);
      blockTypeChangeHandler.setType(entityReference, `${blockName}.${block2Name}`, newType);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference), `${blockName}.${block2Name}.${propertyName}`), null);
    });
  });
});