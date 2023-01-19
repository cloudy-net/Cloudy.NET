import assert from 'assert';
import { } from './polyfiller.js';
import stateManager from '../data/state-manager.js';
import simpleChangeHandler from '../data/change-handlers/simple-change-handler.js';

describe('state-manager.js', () => {
  describe('simple scenario', () => {
    it('should merge simple change if < 5 min old', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.changes = [
        { '$type': 'simple', date: Date.now(), path: ['propertyName'], value: 'lorem' },
      ];

      stateManager.getOrCreateLatestChange(state, 'simple', ['propertyName']);

      assert.equal(state.changes.length, 1);
    });
    it('should not merge simple change if > 5 min old', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.changes = [
        { '$type': 'simple', date: Date.now() - 1000000, path: ['propertyName'], value: 'lorem' },
      ];

      stateManager.getOrCreateLatestChange(state, 'simple', ['propertyName']);

      assert.equal(state.changes.length, 2);
    });
    it('should merge block type change if < 5 min old', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.changes = [
        { '$type': 'embeddedblock', date: Date.now(), path: ['blockName'], value: 'lorem' },
      ];

      stateManager.getOrCreateLatestChange(state, 'embeddedblock', ['blockName']);

      assert.equal(state.changes.length, 1);
    });
    it('should not merge block type change if > 5 min old', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.changes = [
        { '$type': 'embeddedblock', date: Date.now() - 1000000, path: ['blockName'], value: 'lorem' },
      ];

      stateManager.getOrCreateLatestChange(state, 'embeddedblock', ['blockName']);

      assert.equal(state.changes.length, 2);
    });
  });
  describe('complex scenario', () => {
    it('should not merge with change if separated by block change', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.changes = [
        { '$type': 'simple', date: Date.now(), path: ['blockName', 'propertyName'], value: 'lorem' },
        { '$type': 'embeddedblock', date: Date.now(), path: ['blockName'], type: 'ipsum' },
      ];

      stateManager.getOrCreateLatestChange(state, 'simple', ['propertyName']);

      assert.equal(state.changes.length, 3);
    });
    it('should not merge with change if separated by nested block change', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.changes = [
        { '$type': 'simple', date: Date.now(), path: ['blockName', 'nestedBlockName', 'propertyName'], value: 'lorem' },
        { '$type': 'embeddedblock', date: Date.now(), path: ['blockName'], type: 'ipsum' },
      ];

      stateManager.getOrCreateLatestChange(state, 'simple', ['blockName', 'nestedBlockName', 'propertyName']);

      assert.equal(state.changes.length, 3);
    });
  });
});
  