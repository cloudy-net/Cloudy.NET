import { useState, useEffect } from './lib/preact.hooks.module.js';
import html from './html.js';
import ContentTypeProvider from './ContentAppSupport/utils/content-type-provider.js';
import List from './ListSupport/list.js';
import ListItem from './ListSupport/list-item.js';

function App(props) {
    const [editingContentType, editContentType] = useState(null);
    const [items, setItems] = useState([]);

    useEffect(() => {
        ContentTypeProvider.getAll().then(data => setItems(data));
    }, []);

    return html`
        <cloudy-ui-portal>
            <cloudy-ui-portal-nav>
                <cloudy-ui-portal-nav-title>${props.title}<//>
                <div>

                </div>
            <//>
            <cloudy-ui-app>
                <cloudy-ui-blade>
                    <cloudy-ui-blade-title><cloudy-ui-blade-title-text>What to edit<//><//>
                    <${List}>
				        ${items.map(result => html`<${ListItem} active=${result == editingContentType} text=${result.name} onclick=${() => editContentType(result)} />`)}
                    <//>
                <//>
            <//>
        <//>
    `;
}

export default App;
