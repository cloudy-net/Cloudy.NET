import ViewChanges from "../data/view-changes.js";
import { html, useContext, useState } from "../preact-htm/standalone.module.js";
import EntityContext from "./entity-context.js";


const Changes = () => {
  const { changes, sourceConflicts } = useContext(EntityContext);
  
  const [showHistory, setShowHistory] = useState();

  return html`
    ${
      sourceConflicts.length ?
      html`<div class="alert alert-info">
        <strong>The source and/or model has changed since you started editing.</strong><br/>
        <a style="text-decoration: underline;" tabIndex="0" onClick=${() => setShowHistory(!showHistory)}>Review the change.s</a> before you continue.
      </div>` :
      html`<div style="text-align: right;">
        ${changes.length ? html`<a tabIndex="0" onClick=${() => setShowHistory(!showHistory)}>View change.s</a>` : html`<div style="white-space: pre"> <//>`}
      </div>`
    }
    ${showHistory && html`<${ViewChanges} />`}
  `
};

export default Changes;