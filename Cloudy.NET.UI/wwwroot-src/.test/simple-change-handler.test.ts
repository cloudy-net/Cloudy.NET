import stateManager from '../src/data/state-manager';
import simpleChangeHandler from '../src/data/change-handlers/simple-change-handler';
import blockTypeHandler from '../src/data/change-handlers/block-type-handler';
import statePersister from '../src/data/state-persister';



test('intermediate value', () => {
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
        [propertyName]: initialValue
      }
    }
  });

  expect(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference)!, propertyName)).toBe(initialValue);
  simpleChangeHandler.setValue(entityReference, propertyName, newValue);
  expect(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference)!, propertyName)).toBe(newValue);
});
test('change should be deleted if it equals source', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const { entityReference } = stateManager.createStateForNewEntity('page');
  const propertyName = 'TestProperty';
  const initialValue = 'lorem';

  stateManager.replace({
    ...stateManager.getState(entityReference)!,
    source: {
      value: {
        [propertyName]: initialValue
      }
    }
  });

  simpleChangeHandler.setValue(entityReference, propertyName, initialValue);
  expect(stateManager.getState(entityReference)!.history.length).toBe(0);
});
test('change should not be deleted if it equals source but previous changes exist', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const { entityReference } = stateManager.createStateForNewEntity('page');
  const propertyName = 'TestProperty';
  const initialValue = 'lorem';

  stateManager.replace({
    ...stateManager.getState(entityReference)!,
    source: {
      value: {
        [propertyName]: initialValue
      }
    },
    history: [
      { '$type': 'simple', date: Date.now() - 10 * 60 * 1000, path: propertyName, value: '' },
    ]
  });

  simpleChangeHandler.setValue(entityReference, propertyName, initialValue);
  expect(stateManager.getState(entityReference)!.history.length).toBe(2);
});
test('intermediate value, deep path', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const { entityReference } = stateManager.createStateForNewEntity('page');
  const blockName = 'Block1';
  const block2Name = 'Block2';
  const propertyName = 'TestProperty';
  const initialValue = 'lorem';
  const newValue = 'ipsum';

  stateManager.replace({
    ...stateManager.getState(entityReference)!,
    source: {
      value: {
        [blockName]: {
          Value: {
            [block2Name]: {
              Value: {
                [propertyName]: initialValue
              }
            }
          }
        }
      }
    }
  });

  expect(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference)!, `${blockName}.${block2Name}.${propertyName}`)).toBe(initialValue);
  simpleChangeHandler.setValue(entityReference, `${blockName}.${block2Name}.${propertyName}`, newValue);
  expect(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference)!, `${blockName}.${block2Name}.${propertyName}`)).toBe(newValue);
});

test('simple change after changing block type', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const { entityReference } = stateManager.createStateForNewEntity('page');
  const newType = 'BlockType';
  const blockName = 'Block';
  const propertyName = 'TestProperty';
  const initialValue = 'lorem';
  const newValue = 'ipsum';

  stateManager.replace({
    ...stateManager.getState(entityReference)!,
    source: {
      value: {
        [blockName]: {
          Value: {
            [propertyName]: initialValue
          }
        }
      }
    }
  });

  blockTypeHandler.setType(entityReference, blockName, newType);
  simpleChangeHandler.setValue(entityReference, `${blockName}.${propertyName}`, newValue);
  expect(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference)!, `${blockName}.${propertyName}`)).toBe(newValue);
});
test('changing block type after simple change', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const { entityReference } = stateManager.createStateForNewEntity('page');
  const newType = 'BlockType';
  const blockName = 'Block1';
  const block2Name = 'Block2';
  const propertyName = 'TestProperty';
  const initialValue = 'lorem';
  const newValue = 'ipsum';

  stateManager.replace({
    ...stateManager.getState(entityReference)!,
    source: {
      value: {
        [blockName]: {
          Value: {
            [block2Name]: {
              Value: {
                [propertyName]: initialValue
              }
            }
          }
        }
      }
    }
  });

  simpleChangeHandler.setValue(entityReference, `${blockName}.${block2Name}.${propertyName}`, newValue);
  expect(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference)!, `${blockName}.${block2Name}.${propertyName}`)).toBe(newValue);
  blockTypeHandler.setType(entityReference, `${blockName}.${block2Name}`, newType);
  expect(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference)!, `${blockName}.${block2Name}.${propertyName}`)).toBe(undefined);
  simpleChangeHandler.setValue(entityReference, `${blockName}.${block2Name}.${propertyName}`, newValue);
  expect(stateManager.getState(entityReference)!.history.length).toBe(3);
  expect(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference)!, `${blockName}.${block2Name}.${propertyName}`)).toBe(newValue);
  blockTypeHandler.setType(entityReference, blockName, newType);
  expect(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference)!, `${blockName}.${block2Name}.${propertyName}`)).toBe(undefined);
});
test('intermediate value should be cleared when changing parents parents block type', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const { entityReference } = stateManager.createStateForNewEntity('page');
  const newType = 'BlockType';
  const blockName = 'Block1';
  const block2Name = 'Block2';
  const propertyName = 'TestProperty';
  const initialValue = 'lorem';
  const newValue = 'ipsum';

  stateManager.replace({
    ...stateManager.getState(entityReference)!,
    source: {
      value: {
        [blockName]: {
          Value: {
            [block2Name]: {
              Value: {
                [propertyName]: initialValue
              }
            }
          }
        }
      }
    }
  });

  simpleChangeHandler.setValue(entityReference, `${blockName}.${block2Name}.${propertyName}`, newValue);
  blockTypeHandler.setType(entityReference, `${blockName}.${block2Name}`, newType);
  expect(simpleChangeHandler.getIntermediateValue(stateManager.getState(entityReference)!, `${blockName}.${block2Name}.${propertyName}`)).toBe(undefined);
});
