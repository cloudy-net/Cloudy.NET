import Button from '../components/button/button.js';
import html from '../util/html.js';
import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';
import stateContext from '../edit-content/total-changes-context.js';

function TotalChangesButton({ onClick }) {
    const totalChanges = useContext(stateContext);

    return html`
        <${Button} cssClass="primary" text=${!totalChanges ? 'Show changes' : totalChanges == 1 ? `${totalChanges} change` : `${totalChanges} changes`} disabled=${!totalChanges} onClick=${onClick}/>
    `;
}

export default TotalChangesButton;