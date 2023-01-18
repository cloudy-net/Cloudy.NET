import assert from 'assert';
import { } from './polyfiller.js';
import stateManager from '../data/state-manager.js';
import simpleChangeHandler from '../data/change-handlers/simple-change-handler.js';
import embeddedBlockChangeHandler from '../data/change-handlers/embedded-block-change-handler.js';
import arrayStartsWith from '../util/array-starts-with.js';

describe('array-starts-with.js', () => {
  it('a equals b', async () => {
    assert.equal(arrayStartsWith([1, 2, 3], [1, 2, 3]), true);
  });
  it('a is shorter than b', async () => {
    assert.equal(arrayStartsWith([1, 2, 3], [1, 2, 3, 4]), false);
  });
  it('a starts with b', async () => {
    assert.equal(arrayStartsWith([1, 2, 3], [1, 2]), true);
  });
});