import Conflicts from "../data/conflicts.js";
import History from "../data/history.js";
import html from '@src/html-init.js';
import { useContext, useState } from 'preact/hooks';
import EntityContext from "./contexts/entity-context";


const Changes = () => {
  const { state } = useContext(EntityContext);

  const [showHistory, setShowHistory] = useState();

  if (state.conflicts.length) {
    return html`<div class="alert alert-info">
        <strong>The source and/or model has changed since you started editing.</strong><br/>
        You must review these changes before you continue.
      </div>
      <${Conflicts}/>
    `;
  }

  if(state.changes.length == 0){
    return html`<div style="white-space: pre"> <//>`;
  }

  return html`
    <div style="text-align: right;"><a tabIndex="0" onClick=${() => setShowHistory(!showHistory)}>View changes</a></div>
    ${showHistory && html`<${History} />`}
  `;
};

export default Changes;