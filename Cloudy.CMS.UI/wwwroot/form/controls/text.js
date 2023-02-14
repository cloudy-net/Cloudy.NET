import { html, useContext } from '../../preact-htm/standalone.module.js';
import EntityContext from '../entity-context.js';
import simpleChangeHandler from '../../data/change-handlers/simple-change-handler.js';
import ValidationManager from '../../data/validation-manager.js';

const Control = ({ name, path, validators }) => {
  const { entityReference, state } = useContext(EntityContext);

  return html`
      <input
        type="text"
        class=${`form-control ${ ValidationManager.getValidationClass(state.validationResults, path) } `}
        id=${`field-${name}`}
        name=${name}
        value=${simpleChangeHandler.getIntermediateValue(state, path)}
        onInput=${(e) => simpleChangeHandler.setValue(entityReference, path, e.target.value, validators)}
      />`;
}

export default Control;