import stateManager from '../src/data/state-manager';
import statePersister from '../src/data/state-persister';
import embeddedBlockListHandler from '../src/data/change-handlers/embedded-block-list-handler';

  test('intermediate value', () => {
    global.localStorage.clear();
    stateManager.states = statePersister.loadStates();
    const { entityReference } = stateManager.createStateForNewEntity('page');
    const propertyName = 'TestProperty';
    const blockType = 'BlockType';

    stateManager.replace({
      ...stateManager.getState(entityReference)!,
      source: {
        value: {
          [propertyName]: [{ Type: blockType }, { Type: blockType }]
        }
      }
    });

    expect(embeddedBlockListHandler.getIntermediateValue(stateManager.getState(entityReference)!, propertyName)).toStrictEqual([{ key: '0', type: blockType }, { key: '1', type: blockType }]);
    const item = embeddedBlockListHandler.add(entityReference, propertyName, blockType);
    expect(embeddedBlockListHandler.getIntermediateValue(stateManager.getState(entityReference)!, propertyName)).toStrictEqual([{ key: '0', type: blockType }, { key: '1', type: blockType }, { key: item.key, type: blockType }]);
  });
  test('should not merge change', () => {
    global.localStorage.clear();
    stateManager.states = statePersister.loadStates();
    const propertyName = 'TestProperty';
    const blockType = 'BlockType';
    const state = stateManager.createStateForNewEntity('page');
    state.history = [
      { '$type': 'embeddedblocklist.add', date: Date.now(), path: propertyName, key: 'lorem', type: blockType },
    ];

    embeddedBlockListHandler.add(state.entityReference, propertyName, blockType);

    expect(state.history.length).toBe(2);
  });
  test('remove new element', () => {
    global.localStorage.clear();
    stateManager.states = statePersister.loadStates();
    const { entityReference } = stateManager.createStateForNewEntity('page');
    const propertyName = 'TestProperty';
    const blockType = 'BlockType';

    const item = embeddedBlockListHandler.add(entityReference, propertyName, blockType);
    expect(embeddedBlockListHandler.getIntermediateValue(stateManager.getState(entityReference)!, propertyName)).toStrictEqual([{ key: item.key, type: item.type }]);
    embeddedBlockListHandler.remove(entityReference, propertyName, item.key!, item.type);
    expect(embeddedBlockListHandler.getIntermediateValue(stateManager.getState(entityReference)!, propertyName)).toStrictEqual([]);
  });