import { html, useContext } from '../../preact-htm/standalone.module.js';
import stateManager from '../../data/state-manager.js';
import EntityContext from '../entity-context.js';
import simpleChangeHandler from '../../data/change-handlers/simple-change-handler.js';

const Control = ({ name, path }) => {
  const { entityReference, state } = useContext(EntityContext);

  const onchange = event => {
    simpleChangeHandler.setValue(entityReference, path, event.target.checked)
  };
  return html`<div class="form-check">
      <input
        type="checkbox"
        class="form-check-input"
        id=${name}
        name=${name}
        value=${simpleChangeHandler.getIntermediateValue(state, path)}
        onInput=${onchange}
      />
    </div>`;
}

export default Control;