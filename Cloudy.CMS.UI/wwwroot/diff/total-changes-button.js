import Button from '../components/button/button.js';
import html from '../util/html.js';
import showDiffContext from './show-diff-context.js';
import { useContext } from '../lib/preact.hooks.module.js';

function TotalChangesButton() {
    const [, showDiff] = useContext(showDiffContext);

    return html`
        <${Button} text='Show changes' onclick=${() => showDiff(true)}/>
    `;
}

export default TotalChangesButton;