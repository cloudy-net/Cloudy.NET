import { html, useContext } from '../../preact-htm/standalone.module.js';
import stateManager from '../../data/state-manager.js';
import EntityContext from '../entity-context.js';
import simpleChangeHandler from '../../data/change-handlers/simple-change-handler.js';

const Control = ({ name, path }) => {
  const { contentReference, state } = useContext(EntityContext);

  const onchange = event => {
    simpleChangeHandler.registerChange(stateManager, contentReference, path, event.target.value)
  };
  return html`<div>
      <input
        type="text"
        class="form-control"
        id=${`field-${name}`}
        name=${name}
        defaultValue=${simpleChangeHandler.getIntermediateValue(state, path)}
        onInput=${onchange}
      />
    </div>`;
}

export default Control;