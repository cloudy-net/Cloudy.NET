import assert from 'assert';
import getReferenceValue from '../util/get-reference-value.js';

describe('get-reference-value.js', () => {
  it('simple property', async () => {
    const propertyName = 'lorem';
    const propertyValue = 'ipsum';

    const state = {
      referenceValues: {
        [propertyName]: propertyValue
      }
    };
    assert.equal(getReferenceValue(state, propertyName), propertyValue);
  });
  it('nested property', async () => {
    const blockName = 'dolor';
    const nestedBlockName = 'dolor';
    const propertyName = 'lorem';
    const propertyValue = 'ipsum';

    const state = {
      referenceValues: {
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
    };
    assert.equal(getReferenceValue(state, `${blockName}.${nestedBlockName}.${propertyName}`), propertyValue);
  });
  it('nested property in null block', async () => {
    const blockName = 'dolor';
    const nestedBlockName = 'dolor';
    const propertyName = 'lorem';
    const propertyValue = null;

    const state = {
      referenceValues: {
        [blockName]: {
          Value: {
            [nestedBlockName]: null
          }
        }
      }
    };
    assert.equal(getReferenceValue(state, `${blockName}.${nestedBlockName}.${propertyName}`), propertyValue);
  });
});