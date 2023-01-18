import assert from 'assert';
import { } from './polyfiller.js';
import stateManager from '../data/state-manager.js';
import simpleChangeHandler from '../data/change-handlers/simple-change-handler.js';
import embeddedBlockChangeHandler from '../data/change-handlers/embedded-block-change-handler.js';

describe('state-manager.js', () => {
  describe('init', () => {
    it('create new state', () => {
      const state = stateManager.createStateForNewContent('page');
      assert.ok(state);
    });
  });
  describe('simple changes', () => {
    describe('simple scenario', () => {
      it('intermediate value', () => {
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

        assert.equal(0, stateManager.getState(contentReference).changes.length);
        assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [propertyName]), initialValue, 'Initial value should be returned as there are no intermediate changes registered');

        simpleChangeHandler.registerChange(stateManager, contentReference, [propertyName], newValue);

        assert.equal(1, stateManager.getState(contentReference).changes.length);
        assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [propertyName]), newValue, 'New value should be returned as an intermediate change has been registered');
      });
      it('intermediate value, deep path', () => {
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
              [block2Name]: {
                [propertyName]: initialValue
              }
            }
          }
        });

        assert.equal(0, stateManager.getState(contentReference).changes.length);
        assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [blockName, block2Name, propertyName]), initialValue, 'Initial value should be returned as there are no intermediate changes registered');

        simpleChangeHandler.registerChange(stateManager, contentReference, [blockName, block2Name, propertyName], newValue);

        assert.equal(1, stateManager.getState(contentReference).changes.length);
        assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [blockName, block2Name, propertyName]), newValue, 'New value should be returned as an intermediate change has been registered');
      });
    });
  });
  describe('complex scenario', () => {
    it('intermediate value should not be cleared when making a simple change after changing block type', () => {
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
            [propertyName]: initialValue
          }
        }
      });

      embeddedBlockChangeHandler.setType(stateManager, contentReference, [blockName], newType);
      simpleChangeHandler.registerChange(stateManager, contentReference, [blockName, propertyName], newValue);

      assert.equal(2, stateManager.getState(contentReference).changes.length, 'Number of registered changes should be 2');
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [blockName, propertyName]), newValue, 'New value expected');
    });
    it('intermediate value should be cleared when changing parent embedded block type', () => {
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
            [block2Name]: {
              [propertyName]: initialValue
            }
          }
        }
      });

      embeddedBlockChangeHandler.setType(stateManager, contentReference, [blockName, block2Name], newType);
      simpleChangeHandler.registerChange(stateManager, contentReference, [blockName, block2Name, propertyName], newValue);

      assert.equal(2, stateManager.getState(contentReference).changes.length, 'Number of registered changes should be 2');
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [blockName, block2Name, propertyName]), newValue, 'New value expected');
    });
    it('intermediate value should be cleared when changing parents parents embedded block type', () => {
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
            [block2Name]: {
              [propertyName]: initialValue
            }
          }
        }
      });

      simpleChangeHandler.registerChange(stateManager, contentReference, [blockName, block2Name, propertyName], newValue);
      embeddedBlockChangeHandler.setType(stateManager, contentReference, [blockName, block2Name], newType);

      assert.equal(2, stateManager.getState(contentReference).changes.length, 'Number of registered changes should be 2');
      assert.equal(simpleChangeHandler.getIntermediateValue(stateManager.getState(contentReference), [blockName, block2Name, propertyName]), null, 'Cleared value expected');
    });
  });
});