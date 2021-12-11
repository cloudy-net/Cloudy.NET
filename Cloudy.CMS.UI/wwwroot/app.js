import { useState, useEffect } from './lib/preact.hooks.module.js';
import html from './html.js';
import ContentTypeProvider from './ContentAppSupport/utils/content-type-provider.js';
import List from './ListSupport/list.js';
import ListItem from './ListSupport/list-item.js';
import ListContent from './list-content.js';

function App(props) {
    const [editingContentType, editContentType] = useState(null);
    const [items, setItems] = useState([]);

    useEffect(() => {
        ContentTypeProvider.getAll().then(items => setItems(items));
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
				        ${items.map(item => html`<${ListItem} active=${item == editingContentType} text=${item.name} onclick=${() => editContentType(item)} />`)}
                    <//>
                    ${editingContentType && html`<${ListContent} contentType=${editingContentType}/>`}
                <//>
            <//>
        <//>
    `;
}

export default App;
