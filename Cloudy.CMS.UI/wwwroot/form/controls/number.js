import { html, useContext } from '../../preact-htm/standalone.module.js';
import stateManager from '../../data/state-manager.js';
import EntityContext from '../entity-context.js';
import simpleChangeHandler from '../../data/change-handlers/simple-change-handler.js';

const Control = ({ name, path }) => {
  const { entityReference, state } = useContext(EntityContext);

  const onchange = event => {
    if (!isNaN(event.target.value)) {
      simpleChangeHandler.setValue(entityReference, path, parseInt(event.target.value))
    }
  };
  return html`<div>
      <input
        type="number"
        pattern="[0-9]+"
        class="form-control"
        id=${name}
        name=${name}
        value=${simpleChangeHandler.getIntermediateValue(state, path)}
        onInput=${onchange}
      />
    </div>`;
}

export default Control;