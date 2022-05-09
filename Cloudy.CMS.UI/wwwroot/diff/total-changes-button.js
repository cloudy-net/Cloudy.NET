import Button from '../components/button/button.js';
import html from '../util/html.js';
import { useContext } from '../lib/preact.hooks.module.js';
import totalChangesContext from '../edit-content/total-changes-context.js';

function TotalChangesButton({ onClick }) {
    const totalChanges = useContext(totalChangesContext);

    return html`
        <${Button} cssClass=${totalChanges && 'primary'} text=${!totalChanges ? 'No changes' : totalChanges == 1 ? `${totalChanges} change` : `${totalChanges} changes`} onClick=${onClick}/>
    `;
}

export default TotalChangesButton;