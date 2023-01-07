import { html, useContext } from '../../preact-htm/standalone.module.js';
import stateManager from '../../data/state-manager.js';
import EntityContext from '../entity-context.js';
import getIntermediateSimpleValue from '../../util/get-intermediate-simple-value.js';

const Control = ({ name, path }) => {
  const { contentReference, state } = useContext(EntityContext);

  const onchange = event => {
    stateManager.registerSimpleChange(contentReference, path, event.target.value)
  };
  return html`<div>
      <textarea
        type="text"
        class="form-control"
        id=${`field-${name}`}
        name=${name}
        defaultValue=${getIntermediateSimpleValue(state, path)}
        onInput=${onchange}
      />
    </div>`;
}

export default Control;