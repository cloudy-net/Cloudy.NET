import Button from '../components/button/button.js';
import html from '../util/html.js';
import showDiffContext from './show-diff-context.js';
import pendingChangesContext from './pending-changes-context.js';
import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';

function TotalChangesButton({ changeCount }) {
    return html`
        <${Button} text=${!changeCount ? 'Show changes' : changeCount == 1 ? `${changeCount} change` : `${changeCount} changes`} disabled=${!changeCount}/>
    `;
}

export default TotalChangesButton;