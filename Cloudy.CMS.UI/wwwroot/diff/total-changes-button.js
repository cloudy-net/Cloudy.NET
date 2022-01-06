import Button from '../components/button/button.js';
import html from '../util/html.js';
import showDiffContext from './show-diff-context.js';
import pendingChangesContext from './pending-changes-context.js';
import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';

function TotalChangesButton() {
    const [, , , setShowDiffBlade] = useContext(showDiffContext);
    const [pendingChanges] = useContext(pendingChangesContext);
    const [changeText, setChangeText] = useState('Show changes');

    useEffect(() => {
        setChangeText(!pendingChanges?.length ? 'Show changes' : (pendingChanges.length == 1 ? `${pendingChanges.length} change` : `${pendingChanges.length} changes`))
    }, [pendingChanges])

    return html`
        <${Button} text=${changeText} onclick=${() => setShowDiffBlade(true)} disabled=${!pendingChanges?.length}/>
    `;
}

export default TotalChangesButton;