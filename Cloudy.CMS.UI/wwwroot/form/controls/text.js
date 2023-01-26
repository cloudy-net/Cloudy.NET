import { html, useContext } from '../../preact-htm/standalone.module.js';
import stateManager from '../../data/state-manager.js';
import EntityContext from '../entity-context.js';
import simpleChangeHandler from '../../data/change-handlers/simple-change-handler.js';
import ValidationManager from '../../data/validation-manager.js';

const Control = ({ name, path, validators }) => {
  const { entityReference, state } = useContext(EntityContext);

  const onchange = event => {
    simpleChangeHandler.setValueAndValidate(entityReference, path, event.target.value, validators)
  };
  return html`
      <input
        required
        type="text"
        class=${`form-control ${ ValidationManager.isInvalidForPath(state.validationResults, path) ? 'is-invalid' : '' }`}
        id=${`field-${name}`}
        name=${name}
        value=${simpleChangeHandler.getIntermediateValue(state, path)}
        onInput=${onchange}
      />`;
}

export default Control;