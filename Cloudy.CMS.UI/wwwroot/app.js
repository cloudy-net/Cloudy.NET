import html from './html.js';
import Nav from './nav.js';

function App(props) {
    return html`
    <Root>
        <${Nav}/>
        <h1>Hello ${props.name}!</h1>
    <//>
    `;
}

export default App;
