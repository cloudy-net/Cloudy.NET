import { } from './lib/preact.debug.module.js';
import html from './util/html.js';
import ListContentTypes from './list-content-types/list-content-types.js';
import ContentTypesContextProvider from './list-content-types/content-types-context-provider.js';
import ListContent from './list-content-types/list-content.js';
import EditContent from './edit-content/edit-content.js';
import PopupMenuContextProvider from './components/popup-menu/popup-menu-context-provider.js';
import TotalChangesButton from './diff/total-changes-button.js';
import { useState } from './lib/preact.hooks.module.js';
import contentStateManager from './edit-content/content-state-manager.js';

function App({ title }) {
    const [listingContent, listContent] = useState(null);
    const [editingContent, editContent] = useState(null);

    return html`
        <${PopupMenuContextProvider}>
            <${ContentTypesContextProvider}>
                <cloudy-ui-portal>
                    <cloudy-ui-portal-nav>
                        <cloudy-ui-portal-nav-title>${title}<//>
                        <div>
                            <${TotalChangesButton}/>
                        </div>
                    <//>
                    <cloudy-ui-app>
                        <${ListContentTypes} activeContentType=${listingContent} onSelectContentType=${contentType => listContent(contentType)}/>
                        ${listingContent ? html`<${ListContent} contentType=${listingContent} onEditContent=${contentReference => editContent(contentReference)} onNewContent=${contentType => editContent(contentStateManager.createNewContent(contentType))} onClose=${() => listContent(null)}/>` : null}
                        <${EditContent} contentReference=${editingContent}/>
                    <//>
                <//>
            <//>
        <//>
    `;
}

export default App;
