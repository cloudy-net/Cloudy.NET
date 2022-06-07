import Button from '../components/button/button.js';
import html from '../util/html.js';
import { useEffect, useState } from '../lib/preact.hooks.module.js';
import stateManager from '../edit-content/state-manager.js';

function TotalChangesButton({ onClick }) {
    const [totalChanges, setTotalChanges] = useState(stateManager.getAll().length);

    useEffect(() => {
        const callback = () => {
            setTotalChanges(stateManager.getAll().length);
        };
        stateManager.onAnyStateChange(callback);

        return () => {
            stateManager.offAnyStateChange(callback);
        };
    }, []);

    return html`
        <${Button} text=${!totalChanges ? 'No changes' : totalChanges == 1 ? `${totalChanges} change` : `${totalChanges} changes`} onClick=${onClick}/>
    `;
}

export default TotalChangesButton;