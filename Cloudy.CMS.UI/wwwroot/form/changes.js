import UndoHistory from "../data/view-changes.js";
import { html, useContext, useState } from "../preact-htm/standalone.module.js";
import EntityContext from "./entity-context.js";


const ChangedContentWarning = () => {
  const { state, mergedChanges } = useContext(EntityContext);
  
  if(!state.newVersion){
    // return;
  }

  const [showHistory, setShowHistory] = useState();

  return html`
    ${
      true ?
      html`<div style="text-align: right;">${mergedChanges.length ? html`<a tabIndex="0" onClick=${() => setShowHistory(!showHistory)}>View changes</a>` : html`<div style="white-space: pre"> <//>`}</div>` :
      html`<div class="alert alert-info">
        <strong>The source has changed since you started editing.</strong><br/>
        <a style="text-decoration: underline;" tabIndex="0" onClick=${() => setShowHistory(!showHistory)}>Review the changes</a> before you continue.
      </div>`
    }
    ${showHistory && html`<${UndoHistory} />`}
  `
};

export default ChangedContentWarning;