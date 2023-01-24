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
        { '$type': 'simple', date: Date.now(), path: 'propertyName', value: 'lorem' },
      ];

      stateManager.getOrCreateLatestChange(state, 'simple', 'propertyName');

      assert.equal(state.changes.length, 1);
    });
    it('should not merge simple change if > 5 min old', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.changes = [
        { '$type': 'simple', date: Date.now() - 1000000, path: 'propertyName', value: 'lorem' },
      ];

      stateManager.getOrCreateLatestChange(state, 'simple', 'propertyName');

      assert.equal(state.changes.length, 2);
    });
    it('should merge block type change if < 5 min old', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.changes = [
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', value: 'lorem' },
      ];

      stateManager.getOrCreateLatestChange(state, 'blocktype', 'blockName');

      assert.equal(state.changes.length, 1);
    });
    it('should not merge block type change if > 5 min old', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.changes = [
        { '$type': 'blocktype', date: Date.now() - 1000000, path: 'blockName', value: 'lorem' },
      ];

      stateManager.getOrCreateLatestChange(state, 'blocktype', 'blockName');

      assert.equal(state.changes.length, 2);
    });
  });
  describe('complex scenario', () => {
    it('should not merge simple change if separated by block change', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.changes = [
        { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'lorem' },
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
      ];

      stateManager.getOrCreateLatestChange(state, 'simple', 'propertyName');

      assert.equal(state.changes.length, 3);
    });
    it('should not merge simple change if separated by nested block change', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.changes = [
        { '$type': 'simple', date: Date.now(), path: 'blockName.nestedBlockName.propertyName', value: 'lorem' },
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
      ];

      stateManager.getOrCreateLatestChange(state, 'simple', 'blockName.nestedBlockName.propertyName');

      assert.equal(state.changes.length, 3);
    });
    it('should merge block type change if separated by irrelevant simple change', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.changes = [
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
        { '$type': 'simple', date: Date.now(), path: 'propertyName', value: 'lorem' },
      ];

      stateManager.getOrCreateLatestChange(state, 'blocktype', 'blockName');

      assert.equal(state.changes.length, 2);
    });
    it('should not merge block type change if separated by nested simple change', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.changes = [
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
        { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'lorem' },
      ];

      stateManager.getOrCreateLatestChange(state, 'blocktype', 'blockName');

      assert.equal(state.changes.length, 3);
    });
  });

  describe('getMergedChanges', () => {
    it('should return changes', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');

      const changes = [
        { '$type': 'simple', date: Date.now(), path: 'property1Name', value: 'lorem' },
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
        { '$type': 'simple', date: Date.now(), path: 'blockName.property2Name', value: 'dolor' },
      ]

      state.changes = [...changes];

      const result = stateManager.getMergedChanges(state);

      assert.deepEqual(result, changes);
    });
    it('should not take changes cleared by type change', () => {
      global.localStorage.clear();
      stateManager.states = stateManager.loadStates();
      const state = stateManager.createStateForNewContent('page');

      const changes = [
        { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'lorem' },
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
        { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'dolor' },
      ]

      state.changes = [...changes];

      const result = stateManager.getMergedChanges(state);

      assert.deepEqual(result, [changes[1], changes[2]]);
    });
    // it('should merge changes separated by date', () => {
    //   assert.fail('not implemented')
    // });
    // it('should not return changes matching reference value', () => {
    //   assert.fail('not implemented')
    // });
  });
});
  