import Button from '../components/button/button.js';
import html from '../util/html.js';
import showDiffContext from './show-diff-context.js';
import pendingChangesContext from './pending-changes-context.js';
import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';
import stateContext from '../edit-content/total-changes-context.js';

function TotalChangesButton() {
    const totalChanges = useContext(stateContext);

    return html`
        <${Button} text=${!totalChanges ? 'Show changes' : totalChanges == 1 ? `${totalChanges} change` : `${totalChanges} changes`} disabled=${!totalChanges}/>
    `;
}

export default TotalChangesButton;