import stateManager from '../src/data/state-manager';
import changeManager from '../src/data/change-manager';
import statePersister from '../src/data/state-persister';



test('should merge simple change if < 5 min old', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');
  state.history = [
    { '$type': 'simple', date: Date.now(), path: 'propertyName', value: 'lorem' },
  ];

  changeManager.getOrCreateLatestChange(state, 'simple', 'propertyName');

  expect(state.history.length).toBe(1);
});
test('should not merge simple change if > 5 min old', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');
  state.history = [
    { '$type': 'simple', date: Date.now() - 1000000, path: 'propertyName', value: 'lorem' },
  ];

  changeManager.getOrCreateLatestChange(state, 'simple', 'propertyName');

  expect(state.history.length).toBe(2);
});
test('should merge block type change if < 5 min old', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');
  state.history = [
    { '$type': 'blocktype', date: Date.now(), path: 'blockName', value: 'lorem' },
  ];

  changeManager.getOrCreateLatestChange(state, 'blocktype', 'blockName');

  expect(state.history.length).toBe(1);
});
test('should not merge block type change if > 5 min old', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');
  state.history = [
    { '$type': 'blocktype', date: Date.now() - 1000000, path: 'blockName', value: 'lorem' },
  ];

  changeManager.getOrCreateLatestChange(state, 'blocktype', 'blockName');

  expect(state.history.length).toBe(2);
});

test('should not merge simple change if separated by block change', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');
  state.history = [
    { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'lorem' },
    { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
  ];

  changeManager.getOrCreateLatestChange(state, 'simple', 'propertyName');

  expect(state.history.length).toBe(3);
});
test('should not merge simple change if separated by nested block change', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');
  state.history = [
    { '$type': 'simple', date: Date.now(), path: 'blockName.nestedBlockName.propertyName', value: 'lorem' },
    { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
  ];

  changeManager.getOrCreateLatestChange(state, 'simple', 'blockName.nestedBlockName.propertyName');

  expect(state.history.length).toBe(3);
});
test('should merge block type change if separated by irrelevant simple change', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');
  state.history = [
    { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
    { '$type': 'simple', date: Date.now(), path: 'propertyName', value: 'lorem' },
  ];

  changeManager.getOrCreateLatestChange(state, 'blocktype', 'blockName');

  expect(state.history.length).toBe(2);
});
test('should not merge block type change if separated by nested simple change', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');
  state.history = [
    { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
    { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'lorem' },
  ];

  changeManager.getOrCreateLatestChange(state, 'blocktype', 'blockName');

  expect(state.history.length).toBe(3);
});

test('should return changes', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');

  const history = [
    { '$type': 'simple', date: Date.now(), path: 'property1Name', value: 'lorem' },
    { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
    { '$type': 'simple', date: Date.now(), path: 'blockName.property2Name', value: 'dolor' },
  ]

  state.history = [...history];

  const result = changeManager.getChanges(state);

  expect(result).toStrictEqual(history);
});
test('should not return changes cleared by type change', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');

  const history = [
    { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'lorem' },
    { '$type': 'blocktype', date: Date.now(), path: 'blockName', type: 'ipsum' },
    { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'dolor' },
  ]

  state.history = [...history];

  const result = changeManager.getChanges(state);

  expect(result).toStrictEqual([history[1], history[2]]);
});
test('if removing block that is newly added, remove both changes', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');

  const history = [
    { '$type': 'embeddedblocklist.add', date: Date.now(), path: 'listName', key: 'key' },
    { '$type': 'embeddedblocklist.remove', date: Date.now(), path: 'listName', key: 'key' },
  ]

  state.history = [...history];

  const result = changeManager.getChanges(state);

  expect(result).toStrictEqual([]);
});
test('should not return changes cleared by list element removal', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');

  const history = [
    { '$type': 'simple', date: Date.now(), path: 'listName.key.propertyName', value: 'lorem' },
    { '$type': 'embeddedblocklist.remove', date: Date.now(), path: 'listName', key: 'key' },
  ]

  state.history = [...history];

  const result = changeManager.getChanges(state);

  expect(result).toStrictEqual([history[1]]);
});
test('should merge changes on same path', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');

  const history = [
    { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'lorem' },
    { '$type': 'simple', date: Date.now(), path: 'blockName.propertyName', value: 'dolor' },
  ]

  state.history = [...history];

  const result = changeManager.getChanges(state);

  expect(result).toStrictEqual([history[1]]);
});
test('should not return simple changes matching source', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');

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

  expect(result).toStrictEqual([]);
});
test('should not return block type changes matching source', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const state = stateManager.createStateForNewEntity('page');

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

  expect(result).toStrictEqual([]);
});

test('simple property', async () => {
  const propertyName = 'lorem';
  const propertyValue = 'ipsum';

  const state = {
    source: {
      value: {
        [propertyName]: propertyValue
      }
    }
  };
  expect(changeManager.getSourceValue(state.source.value, propertyName)).toBe(propertyValue);
});
test('nested property', async () => {
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
  expect(changeManager.getSourceValue(state.source.value, `${blockName}.${nestedBlockName}.${propertyName}`)).toBe(propertyValue);
});
test('nested property in null block', async () => {
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
  expect(changeManager.getSourceValue(state.source.value, `${blockName}.${nestedBlockName}.${propertyName}`)).toBe(propertyValue);
});
