import { html, useContext } from '../../preact-htm/standalone.module.js';
import stateManager from '../../data/state-manager.js';
import EntityContext from '../entity-context.js';
import simpleChangeHandler from '../../data/change-handlers/simple-change-handler.js';

const Control = ({ name, path }) => {
  const { entityReference, state } = useContext(EntityContext);

  const onchange = event => {
    let value = (event.target.value || '').replace(',', '.');
    if (!!value && !isNaN(value)) {
      simpleChangeHandler.setValue(entityReference, path, parseFloat(value))
    }
  };
  return html`<div>
      <input
        type="number"
        step="any"
        pattern="[0-9]+(.[0-9]+?)?"
        class="form-control"
        id=${name}
        name=${name}
        value=${simpleChangeHandler.getIntermediateValue(state, path)}
        onInput=${onchange}
      />
    </div>`;
}

export default Control;