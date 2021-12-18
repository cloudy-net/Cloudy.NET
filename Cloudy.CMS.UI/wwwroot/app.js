import html from './util/html.js';
import ListContentTypes from './list-content-types/list-content-types.js';
import ListContentTypeContextProvider from './list-content-types/list-content-type-context-provider.js';
import EditContentContextProvider from './edit-content/edit-content-context-provider.js';
import ListContent from './list-content-types/list-content.js';
import EditContent from './edit-content/edit-content.js';
import PopupMenuContextProvider from './components/popup-menu/popup-menu-context-provider.js';
import ShowDiffContextProvider from './diff/show-diff-context-provider.js';
import TotalChangesButton from './diff/total-changes-button.js';
import ShowDiff from './diff/show-diff.js';

function App(props) {
    return html`
        <${PopupMenuContextProvider}>
            <${EditContentContextProvider}>
                <${ListContentTypeContextProvider}>
                    <${ShowDiffContextProvider}>
                        <cloudy-ui-portal>
                            <cloudy-ui-portal-nav>
                                <cloudy-ui-portal-nav-title>${props.title}<//>
                                <div>
                                    <${TotalChangesButton}/>
                                </div>
                            <//>
                            <cloudy-ui-app>
                                <${ListContentTypes}/>
                                <${ListContent}/>
                                <${EditContent}/>
                                <${ShowDiff}/>
                            <//>
                        <//>
                    <//>
                <//>
            <//>
        <//>
    `;
}

export default App;
