import assert from 'assert';
import { } from './polyfiller.js';
import stateManager from '../data/state-manager.js';
import blockTypeChangeHandler from '../data/change-handlers/block-type-change-handler.js';

describe('block-type-change-handler.js', () => {
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
          [propertyName]: {
            Type: initialValue
          }
        }
      });

      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(contentReference), propertyName), initialValue);
      blockTypeChangeHandler.setType(stateManager, contentReference, propertyName, newValue);
      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(contentReference), propertyName), newValue);
    });
    it('clearing value', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const { contentReference } = stateManager.createStateForNewContent('page');
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';
      const newValue = null;

      stateManager.replace({
        ...stateManager.getState(contentReference),
        referenceValues: {
          [propertyName]: {
            Type: initialValue
          }
        }
      });

      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(contentReference), propertyName), initialValue);
      blockTypeChangeHandler.setType(stateManager, contentReference, propertyName, newValue);
      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(contentReference), propertyName), newValue);
    });
  });
  describe('complex scenario', () => {
    it('intermediate value', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const { contentReference } = stateManager.createStateForNewContent('page');
      const blockName = 'Block1';
      const nestedBlockName = 'Block2';
      const initialType = 'lorem';
      const newType = 'ipsum';
      const nestedBlockType = 'NestedBlockType';

      stateManager.replace({
        ...stateManager.getState(contentReference),
        referenceValues: {
          [blockName]: {
            Type: initialType,
            Value: {
              [nestedBlockName]: {
                Type: nestedBlockType
              }
            }
          }
        }
      });

      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(contentReference), blockName), initialType);
      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(contentReference), `${blockName}.${nestedBlockName}`), nestedBlockType);
      blockTypeChangeHandler.setType(stateManager, contentReference, blockName, newType);
      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(contentReference), blockName), newType);
      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(contentReference), `${blockName}.${nestedBlockName}`), null);
    });
  });
});