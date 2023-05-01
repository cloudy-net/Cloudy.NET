import assert from 'assert';
import { } from './polyfiller';
import stateManager from '../src/data/state-manager';
import blockTypeChangeHandler from '../src/data/change-handlers/block-type-handler';
import statePersister from '../src/data/state-persister';

describe('block-type-handler', () => {
  describe('simple scenario', () => {
    it('intermediate value', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const { entityReference } = stateManager.createStateForNewEntity('page');
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';
      const newValue = 'ipsum';

      stateManager.replace({
        ...stateManager.getState(entityReference)!,
        source: {
          value: {
            [propertyName]: {
              Type: initialValue
            }
          }
        }
      });

      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference), propertyName), initialValue);
      blockTypeChangeHandler.setType(entityReference, propertyName, newValue);
      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference), propertyName), newValue);
    });
    it('clearing value', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const { entityReference } = stateManager.createStateForNewEntity('page');
      const propertyName = 'TestProperty';
      const initialValue = 'lorem';
      const newValue = null;

      stateManager.replace({
        ...stateManager.getState(entityReference)!,
        source: {
          value: {
            [propertyName]: {
              Type: initialValue
            }
          }
        }
      });

      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference), propertyName), initialValue);
      blockTypeChangeHandler.setType(entityReference, propertyName, newValue);
      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference), propertyName), newValue);
    });
  });
  describe('complex scenario', () => {
    it('intermediate value', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const { entityReference } = stateManager.createStateForNewEntity('page');
      const blockName = 'Block1';
      const nestedBlockName = 'Block2';
      const initialType = 'lorem';
      const newType = 'ipsum';
      const nestedBlockType = 'NestedBlockType';

      stateManager.replace({
        ...stateManager.getState(entityReference)!,
        source: {
          value: {
            [blockName]: {
              Type: initialType,
              Value: {
                [nestedBlockName]: {
                  Type: nestedBlockType
                }
              }
            }
          }
        }
      });

      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference), blockName), initialType);
      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference), `${blockName}.${nestedBlockName}`), nestedBlockType);
      blockTypeChangeHandler.setType(entityReference, blockName, newType);
      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference), blockName), newType);
      assert.equal(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference), `${blockName}.${nestedBlockName}`), null);
    });
  });
});