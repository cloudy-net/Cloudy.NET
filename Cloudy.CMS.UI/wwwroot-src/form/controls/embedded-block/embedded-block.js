import { html, useContext } from '../../../preact-htm/standalone.module.js';
import stateManager from '../../../data/state-manager.js';
import EntityContext from '../../entity-context.js';
import getIntermediateSimpleValue from '../../../util/get-intermediate-simple-value.js';
import EmbeddedBlockFields from './embedded-block-fields.js';

const Control = ({ name, path, settings: { types } }) => {
  const { contentReference, state } = useContext(EntityContext);
  
  return html`<div>
      <${EmbeddedBlockFields} blockType="Link"/>
    </div>`;
}

export default Control;