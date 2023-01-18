import assert from 'assert';
import { } from './polyfiller.js';
import stateManager from '../data/state-manager.js';

describe('state-manager.js', () => {
  describe('init', () => {
    it('loads test reference entity', () => {
      const entityType = 'page';
      const keyValues = [1];
      const contentReference = { entityType, keyValues, testReferenceValue: { hej: 1 } };
      const state = stateManager.createOrUpdateStateForExistingContent(contentReference);
      assert.strictEqual(state.referenceValues, contentReference.testReferenceValue)
    });
  });
});