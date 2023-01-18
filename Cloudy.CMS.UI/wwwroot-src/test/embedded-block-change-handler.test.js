import assert from 'assert';
import { } from './polyfiller.js';
import stateManager from '../data/state-manager.js';
import embeddedBlockChangeHandler from '../data/change-handlers/embedded-block-change-handler.js';

describe('embedded-block-change-handler.js', () => {
  describe('simple scenario', () => {
    it('intermediate value', () => {
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

      assert.equal(embeddedBlockChangeHandler.getIntermediateType(stateManager.getState(contentReference), [propertyName]), initialValue);
      embeddedBlockChangeHandler.setType(stateManager, contentReference, [propertyName], newValue);
      assert.equal(embeddedBlockChangeHandler.getIntermediateType(stateManager.getState(contentReference), [propertyName]), newValue);
    });
    it('clearing value', () => {
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

      assert.equal(embeddedBlockChangeHandler.getIntermediateType(stateManager.getState(contentReference), [propertyName]), initialValue);
      embeddedBlockChangeHandler.setType(stateManager, contentReference, [propertyName], newValue);
      assert.equal(embeddedBlockChangeHandler.getIntermediateType(stateManager.getState(contentReference), [propertyName]), newValue);
    });
  });
  describe('complex scenario', () => {
    it('intermediate value', () => {
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

      assert.equal(embeddedBlockChangeHandler.getIntermediateType(stateManager.getState(contentReference), [blockName]), initialType);
      assert.equal(embeddedBlockChangeHandler.getIntermediateType(stateManager.getState(contentReference), [blockName, nestedBlockName]), nestedBlockType);
      embeddedBlockChangeHandler.setType(stateManager, contentReference, [blockName], newType);
      assert.equal(embeddedBlockChangeHandler.getIntermediateType(stateManager.getState(contentReference), [blockName]), newType);
      assert.equal(embeddedBlockChangeHandler.getIntermediateType(stateManager.getState(contentReference), [blockName, nestedBlockName]), null);
    });
  });
  // describe('complex scenario', () => {
  //   it('intermediate value should not be cleared when making a simple change after changing block type', () => {
  //     const { contentReference } = stateManager.createStateForNewContent('page');
  //     const newType = 'BlockType';
  //     const blockName = 'Block';
  //     const propertyName = 'TestProperty';
  //     const initialValue = 'lorem';
  //     const newValue = 'ipsum';

  //     stateManager.replace({
  //       ...stateManager.getState(contentReference),
  //       referenceValues: {
  //         [blockName]: {
  //           [propertyName]: initialValue
  //         }
  //       }
  //     });

  //     embeddedBlockChangeHandler.setType(stateManager, contentReference, [blockName], newType);
  //     embeddedBlockChangeHandler.setType(stateManager, contentReference, [blockName, propertyName], newValue);

  //     assert.equal(2, stateManager.getState(contentReference).changes.length, 'Number of registered changes should be 2');
  //     assert.equal(embeddedBlockChangeHandler.getIntermediateType(stateManager.getState(contentReference), [blockName, propertyName]), newValue, 'New value expected');
  //   });
  //   it('intermediate value should be cleared when changing parent embedded block type', () => {
  //     const { contentReference } = stateManager.createStateForNewContent('page');
  //     const newType = 'BlockType';
  //     const blockName = 'Block1';
  //     const nestedBlockName = 'Block2';
  //     const propertyName = 'TestProperty';
  //     const initialValue = 'lorem';
  //     const newValue = 'ipsum';

  //     stateManager.replace({
  //       ...stateManager.getState(contentReference),
  //       referenceValues: {
  //         [blockName]: {
  //           [nestedBlockName]: {
  //             [propertyName]: initialValue
  //           }
  //         }
  //       }
  //     });

  //     embeddedBlockChangeHandler.setType(stateManager, contentReference, [blockName, nestedBlockName], newType);
  //     embeddedBlockChangeHandler.setType(stateManager, contentReference, [blockName, nestedBlockName, propertyName], newValue);

  //     assert.equal(2, stateManager.getState(contentReference).changes.length, 'Number of registered changes should be 2');
  //     assert.equal(embeddedBlockChangeHandler.getIntermediateType(stateManager.getState(contentReference), [blockName, nestedBlockName, propertyName]), newValue, 'New value expected');
  //   });
  //   it('intermediate value should be cleared when changing parents parents embedded block type', () => {
  //     const { contentReference } = stateManager.createStateForNewContent('page');
  //     const newType = 'BlockType';
  //     const blockName = 'Block1';
  //     const nestedBlockName = 'Block2';
  //     const propertyName = 'TestProperty';
  //     const initialValue = 'lorem';
  //     const newValue = 'ipsum';

  //     stateManager.replace({
  //       ...stateManager.getState(contentReference),
  //       referenceValues: {
  //         [blockName]: {
  //           [nestedBlockName]: {
  //             [propertyName]: initialValue
  //           }
  //         }
  //       }
  //     });

  //     embeddedBlockChangeHandler.setType(stateManager, contentReference, [blockName, nestedBlockName, propertyName], newValue);
  //     embeddedBlockChangeHandler.setType(stateManager, contentReference, [blockName, nestedBlockName], newType);

  //     assert.equal(2, stateManager.getState(contentReference).changes.length, 'Number of registered changes should be 2');
  //     assert.equal(embeddedBlockChangeHandler.getIntermediateType(stateManager.getState(contentReference), [blockName, nestedBlockName, propertyName]), null, 'Cleared value expected');
  //   });
  // });
});