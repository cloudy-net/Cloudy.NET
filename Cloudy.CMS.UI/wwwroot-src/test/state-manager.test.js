import assert from 'assert';
import { } from './polyfiller.js';
import stateManager from '../data/state-manager.js';

describe('state-manager.js', () => {
  describe('init', () => {
    it('loads test reference entity', async () => {
      const entityType = 'page';
      const state = await stateManager.createStateForNewContent(entityType);
      assert.ok(state);
    });
  });
});