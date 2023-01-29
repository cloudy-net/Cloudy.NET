import { html, useContext } from '../../preact-htm/standalone.module.js';
import stateManager from '../../data/state-manager.js';
import EntityContext from '../entity-context.js';
import simpleChangeHandler from '../../data/change-handlers/simple-change-handler.js';

const Control = ({ name, path }) => {
  const { entityReference, state } = useContext(EntityContext);

  const onchange = event => {
    simpleChangeHandler.setValue(entityReference, path, event.target.value)
  };
  return html`<div>
      <select
        type="text"
        class="form-control"
        id=${`field-${name}`}
        name=${name}
        value=${simpleChangeHandler.getIntermediateValue(state, path)}
        onInput=${onchange}
      >
      </select>
    </div>`;
}

export default Control;