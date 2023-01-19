import assert from 'assert';
import { } from './polyfiller.js';
import stateManager from '../data/state-manager.js';
import simpleChangeHandler from '../data/change-handlers/simple-change-handler.js';
import blockTypeChangeHandler from '../data/change-handlers/block-type-change-handler.js';

describe('simple-change-handler.js', () => {
  describe('simple scenario', () => {
    it('intermediate value', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const { contentReference } = stateManager.createStateForNewContent('page');
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';
      const newValue = 'ipsum';

      stateManager.replace({
        ...stateManager.getState(contentReference),
        referenceValues: {
          [propertyName]: initialValue
        }
      });

      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [propertyName]), initialValue, 'Initial value should be returned as there are no intermediate changes registered');
      simpleChangeHandler.setValue(stateManager, contentReference, [propertyName], newValue);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [propertyName]), newValue, 'New value should be returned as an intermediate change has been registered');
    });
    it('intermediate value, deep path', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const { contentReference } = stateManager.createStateForNewContent('page');
      const blockName = 'Block1';
      const block2Name = 'Block2';
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';
      const newValue = 'ipsum';

      stateManager.replace({
        ...stateManager.getState(contentReference),
        referenceValues: {
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
      });

      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [blockName, block2Name, propertyName]), initialValue, 'Initial value should be returned as there are no intermediate changes registered');
      simpleChangeHandler.setValue(stateManager, contentReference, [blockName, block2Name, propertyName], newValue);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [blockName, block2Name, propertyName]), newValue, 'New value should be returned as an intermediate change has been registered');
    });
  });
  describe('complex scenario', () => {
    it('simple change after changing block type', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const { contentReference } = stateManager.createStateForNewContent('page');
      const newType = 'BlockType';
      const blockName = 'Block';
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';
      const newValue = 'ipsum';

      stateManager.replace({
        ...stateManager.getState(contentReference),
        referenceValues: {
          [blockName]: {
            Value: {
              [propertyName]: initialValue
            }
          }
        }
      });

      blockTypeChangeHandler.setType(stateManager, contentReference, [blockName], newType);
      simpleChangeHandler.setValue(stateManager, contentReference, [blockName, propertyName], newValue);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [blockName, propertyName]), newValue);
    });
    it('changing block type after simple change', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const { contentReference } = stateManager.createStateForNewContent('page');
      const newType = 'BlockType';
      const blockName = 'Block1';
      const block2Name = 'Block2';
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';
      const newValue = 'ipsum';

      stateManager.replace({
        ...stateManager.getState(contentReference),
        referenceValues: {
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
      });

      simpleChangeHandler.setValue(stateManager, contentReference, [blockName, block2Name, propertyName], newValue);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [blockName, block2Name, propertyName]), newValue);
      blockTypeChangeHandler.setType(stateManager, contentReference, [blockName, block2Name], newType);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [blockName, block2Name, propertyName]), null);
      simpleChangeHandler.setValue(stateManager, contentReference, [blockName, block2Name, propertyName], newValue);
      assert.equal(stateManager.getState(contentReference).changes.length, 3);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [blockName, block2Name, propertyName]), newValue);
      blockTypeChangeHandler.setType(stateManager, contentReference, [blockName], newType);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [blockName, block2Name, propertyName]), null);
    });
    it('intermediate value should be cleared when changing parents parents block type', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const { contentReference } = stateManager.createStateForNewContent('page');
      const newType = 'BlockType';
      const blockName = 'Block1';
      const block2Name = 'Block2';
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';
      const newValue = 'ipsum';

      stateManager.replace({
        ...stateManager.getState(contentReference),
        referenceValues: {
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
      });

      simpleChangeHandler.setValue(stateManager, contentReference, [blockName, block2Name, propertyName], newValue);
      blockTypeChangeHandler.setType(stateManager, contentReference, [blockName, block2Name], newType);
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [blockName, block2Name, propertyName]), null);
    });
  });
});