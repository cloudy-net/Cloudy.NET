import html from './html.js';
import ListContentTypes from './components/list-content-types.js';
import ListContentTypeContextProvider from './list-content-type-context-provider.js';
import ListContent from './components/list-content.js';

function App(props) {
    return html`
        <${ListContentTypeContextProvider}>
            <cloudy-ui-portal>
                <cloudy-ui-portal-nav>
                    <cloudy-ui-portal-nav-title>${props.title}<//>
                    <div>

                    </div>
                <//>
                <cloudy-ui-app>
                    <${ListContentTypes}/>
                    <${ListContent}/>
                <//>
            <//>
        <//>
    `;
}

export default App;
