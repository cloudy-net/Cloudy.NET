import assert from 'assert';
import { } from './polyfiller.js';
import stateManager from '../data/state-manager.js';
import changeManager from '../data/change-manager.js';
import statePersister from '../data/state-persister.js';

describe('state-manager.js', () => {
  describe('simple scenario', () => {
    it('should merge simple change if < 5 min old', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.history = [
        { '$type': 'simple', date: Date.now(), path: 'propertyName', value: 'lorem' },
      ];

      changeManager.getOrCreateLatestChange(state, 'simple', 'propertyName');

      assert.equal(state.history.length, 1);
    });
    it('should not merge simple change if > 5 min old', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.history = [
        { '$type': 'simple', date: Date.now() - 1000000, path: 'propertyName', value: 'lorem' },
      ];

      changeManager.getOrCreateLatestChange(state, 'simple', 'propertyName');

      assert.equal(state.history.length, 2);
    });
    it('should merge block type change if < 5 min old', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.history = [
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', value: 'lorem' },
      ];

      changeManager.getOrCreateLatestChange(state, 'blocktype', 'blockName');

      assert.equal(state.history.length, 1);
    });
    it('should not merge block type change if > 5 min old', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.history = [
        { '$type': 'blocktype', date: Date.now() - 1000000, path: 'blockName', value: 'lorem' },
      ];

      changeManager.getOrCreateLatestChange(state, 'blocktype', 'blockName');

      assert.equal(state.history.length, 2);
    });
  });
  describe('complex scenario', () => {
    it('should not merge simple change if separated by block change', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.history = [
        { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'lorem' },
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
      ];

      changeManager.getOrCreateLatestChange(state, 'simple', 'propertyName');

      assert.equal(state.history.length, 3);
    });
    it('should not merge simple change if separated by nested block change', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.history = [
        { '$type': 'simple', date: Date.now(), path: 'blockName.nestedBlockName.propertyName', value: 'lorem' },
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
      ];

      changeManager.getOrCreateLatestChange(state, 'simple', 'blockName.nestedBlockName.propertyName');

      assert.equal(state.history.length, 3);
    });
    it('should merge block type change if separated by irrelevant simple change', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.history = [
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
        { '$type': 'simple', date: Date.now(), path: 'propertyName', value: 'lorem' },
      ];

      changeManager.getOrCreateLatestChange(state, 'blocktype', 'blockName');

      assert.equal(state.history.length, 2);
    });
    it('should not merge block type change if separated by nested simple change', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const state = stateManager.createStateForNewContent('page');
      state.history = [
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
        { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'lorem' },
      ];

      changeManager.getOrCreateLatestChange(state, 'blocktype', 'blockName');

      assert.equal(state.history.length, 3);
    });
  });

  describe('getChanges', () => {
    it('should return changes', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const state = stateManager.createStateForNewContent('page');

      const history = [
        { '$type': 'simple', date: Date.now(), path: 'property1Name', value: 'lorem' },
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
        { '$type': 'simple', date: Date.now(), path: 'blockName.property2Name', value: 'dolor' },
      ]

      state.history = [...history];

      const result = changeManager.getChanges(state);

      assert.deepEqual(result, history);
    });
    it('should not take changes cleared by type change', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const state = stateManager.createStateForNewContent('page');

      const history = [
        { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'lorem' },
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
        { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'dolor' },
      ]

      state.history = [...history];

      const result = changeManager.getChanges(state);

      assert.deepEqual(result, [history[1], history[2]]);
    });
    it('should merge changes on same path', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const state = stateManager.createStateForNewContent('page');

      const history = [
        { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'lorem' },
        { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'dolor' },
      ]

      state.history = [...history];

      const result = changeManager.getChanges(state);

      assert.deepEqual(result, [history[1]]);
    });
    it('should not return simple changes matching source', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const state = stateManager.createStateForNewContent('page');

      state.source = {
        value: {
          blockName: {
            Value: {
              propertyName: 'lorem'
            }
          }
        }
      };

      const history = [
        { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'lorem' },
      ]

      state.history = [...history];

      const result = changeManager.getChanges(state);

      assert.deepEqual(result, []);
    });
    it('should not return block type changes matching source', () => {
      global.localStorage.clear();
      stateManager.states = statePersister.loadStates();
      const state = stateManager.createStateForNewContent('page');

      state.source = {
        value: {
          blockName: {
            Type: 'lorem'
          }
        }
      };

      const history = [
        { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'lorem' },
      ]

      state.history = [...history];

      const result = changeManager.getChanges(state);

      assert.deepEqual(result, []);
    });
  });
  describe('getSourceValue', () => {
    it('simple property', async () => {
      const propertyName = 'lorem';
      const propertyValue = 'ipsum';

      const state = {
        source: {
          value: {
            [propertyName]: propertyValue
          }
        }
      };
      assert.equal(changeManager.getSourceValue(state.source.value, propertyName), propertyValue);
    });
    it('nested property', async () => {
      const blockName = 'dolor';
      const nestedBlockName = 'dolor';
      const propertyName = 'lorem';
      const propertyValue = 'ipsum';

      const state = {
        source: {
          value: {
            [blockName]: {
              Value: {
                [nestedBlockName]: {
                  Value: {
                    [propertyName]: propertyValue
                  }
                }
              }
            }
          }
        }
      };
      assert.equal(changeManager.getSourceValue(state.source.value, `${blockName}.${nestedBlockName}.${propertyName}`), propertyValue);
    });
    it('nested property in null block', async () => {
      const blockName = 'dolor';
      const nestedBlockName = 'dolor';
      const propertyName = 'lorem';
      const propertyValue = null;

      const state = {
        source: {
          value: {
            [blockName]: {
              Value: {
                [nestedBlockName]: null
              }
            }
          }
        }
      };
      assert.equal(changeManager.getSourceValue(state.source.value, `${blockName}.${nestedBlockName}.${propertyName}`), propertyValue);
    });
  });
});
