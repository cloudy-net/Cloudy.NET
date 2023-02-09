import { html, useContext } from '../../preact-htm/standalone.module.js';
import stateManager from '../../data/state-manager.js';
import EntityContext from '../entity-context.js';
import simpleChangeHandler from '../../data/change-handlers/simple-change-handler.js';
import ValidationManager from '../../data/validation-manager.js';

const Control = ({ name, path, validators }) => {
  const { entityReference, state } = useContext(EntityContext);

  const onchange = event => {
    simpleChangeHandler.setValue(entityReference, path, event.target.value, validators)
  };
  return html`
      <input
        type="text"
        class=${`form-control ${ ValidationManager.getValidationClass(validators, state.validationResults, path) } `}
        id=${`field-${name}`}
        name=${name}
        value=${simpleChangeHandler.getIntermediateValue(state, path)}
        onInput=${onchange}
      />`;
}

export default Control;