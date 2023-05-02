import stateManager from '../src/data/state-manager';
import blockTypeChangeHandler from '../src/data/change-handlers/block-type-handler';
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
        [propertyName]: {
          Type: initialValue
        }
      }
    }
  });

  expect(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference)!, propertyName)).toBe(initialValue);
  blockTypeChangeHandler.setType(entityReference, propertyName, newValue);
  expect(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference)!, propertyName)).toBe(newValue);
});
test('clearing value', () => {
  global.localStorage.clear();
  stateManager.states = statePersister.loadStates();
  const { entityReference } = stateManager.createStateForNewEntity('page');
  const propertyName = 'TestProperty';
  const initialValue = 'lorem';
  const newValue: string | null = null;

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

  expect(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference)!, propertyName)).toBe(initialValue);
  blockTypeChangeHandler.setType(entityReference, propertyName, newValue);
  expect(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference)!, propertyName)).toBe(newValue);
});

test('intermediate value', () => {
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

  expect(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference)!, blockName)).toBe(initialType);
  expect(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference)!, `${blockName}.${nestedBlockName}`)).toBe(nestedBlockType);
  blockTypeChangeHandler.setType(entityReference, blockName, newType);
  expect(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference)!, blockName)).toBe(newType);
  expect(blockTypeChangeHandler.getIntermediateType(stateManager.getState(entityReference)!, `${blockName}.${nestedBlockName}`)).toBe(null);
});