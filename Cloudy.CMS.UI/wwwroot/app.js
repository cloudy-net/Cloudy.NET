import html from './util/html.js';
import ListContentTypes from './list-content-types/list-content-types.js';
import ListContent from './list-content-types/list-content.js';
import EditContent from './edit-content/edit-content.js';
import TotalChangesContextProvider from './edit-content/total-changes-context-provider.js';
import PopupMenuContextProvider from './components/popup-menu/popup-menu-context-provider.js';
import TotalChangesButton from './diff/total-changes-button.js';
import { useEffect, useState } from './lib/preact.hooks.module.js';
import contentStateManager from './edit-content/state-manager.js';
import StateContextProvider from './edit-content/state-context-provider.js';
import PendingChanges from './diff/pending-changes.js';
import contentTypeProvider from './data/content-type-provider.js';
import ShowDiff from './diff/show-diff.js';
import ShowDiffContextProvider from './diff/show-diff-context-provider.js';

function App({ title }) {
    const [contentTypesHasLoaded, setContentTypesHasLoaded] = useState(false);

    useEffect(() => {
        contentTypeProvider.load().then(() => setContentTypesHasLoaded(true));
    }, []);

    if (!contentTypesHasLoaded) {
        return;
    }

    const [listingContent, listContent] = useState(null);
    const [editingContent, editContent] = useState(null);
    const [listingChanges, listChanges] = useState(false);
    const [diffContentReference, setDiffContentReference] = useState(null);

    return html`
        <${PopupMenuContextProvider}>
            <cloudy-ui-portal>
                <cloudy-ui-portal-nav>
                    <cloudy-ui-portal-nav-title>${title}<//>
                    <div>
                        <${TotalChangesContextProvider}>
                            <${TotalChangesButton} onClick=${() => listChanges(true)}/>
                        <//>
                    </div>
                <//>
                <cloudy-ui-app>
                    <${ListContentTypes} renderIf=${!listingChanges} activeContentType=${listingContent} onSelectContentType=${contentType => listContent(contentType)}/>
                    <${ListContent} renderIf=${!listingChanges} activeContentReference=${editingContent} contentType=${listingContent} onEditContent=${(contentReference, nameHint) => editContent(contentStateManager.getOrCreateStateForExistingContent(contentReference, nameHint))} onNewContent=${contentType => editContent(contentStateManager.createStateForNewContent(contentType))} onClose=${() => listContent(null)}/>
                    <${StateContextProvider} renderIf=${!listingChanges && editingContent} contentReference=${editingContent}>
                        <${EditContent} contentReference=${editingContent} onClose=${() => editContent(null)} onReviewChanges=${() => setDiffContentReference(editingContent)}/>
                    <//>

                    <${PendingChanges} renderIf=${listingChanges} onSelect=${contentReference => setDiffContentReference(contentReference)} onClose=${() => { listChanges(null); setDiffContentReference(null) }}/>
                    <${ShowDiffContextProvider} renderIf=${diffContentReference} contentReference=${diffContentReference}>
                        <${ShowDiff} renderIf=${diffContentReference} contentReference=${diffContentReference} onClose=${() => setDiffContentReference(null)}/>
                    <//>
                <//>
            <//>
        <//>
    `;
}

export default App;
