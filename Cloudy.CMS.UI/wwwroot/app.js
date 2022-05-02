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
import ListContentContextProvider from './list-content-types/list-content-context-provider.js';

function App({ title }) {
    const [contentTypesHasLoaded, setContentTypesHasLoaded] = useState(false);

    useEffect(() => {
        contentTypeProvider.load().then(() => setContentTypesHasLoaded(true));
    }, []);

    if (!contentTypesHasLoaded) {
        return;
    }

    const [listingContentTypes, listContentTypes] = useState(true);
    const [listingContent, listContent] = useState(null);
    const [editingContent, editContent] = useState(null);
    const [listingChanges, listChanges] = useState(false);
    const [showingDiff, showDiff] = useState(null);

    const editContentBlade = html`
        <${StateContextProvider} renderIf=${editingContent} contentReference=${editingContent}>
            <${EditContent} contentReference=${editingContent} onClose=${() => { editContent(null); listingContent ? showDiff(null) : null; }} canDiff=${listingContent} onDiff=${() => showDiff(editingContent)}/>
        <//>
    `;

    const showDiffBlade = html`
        <${ShowDiffContextProvider} renderIf=${showingDiff} contentReference=${showingDiff}>
            <${ShowDiff} contentReference=${showingDiff} onClose=${() => { showDiff(null); listingChanges ? editContent(null) : null }} canEdit=${listingChanges} onEdit=${() => editContent(showingDiff)}/>
        <//>
    `;

    return html`
        <${PopupMenuContextProvider}>
            <cloudy-ui-portal>
                <cloudy-ui-portal-nav>
                    <cloudy-ui-portal-nav-title>${title}<//>
                    <div>
                        <${TotalChangesContextProvider}>
                            <${TotalChangesButton} onClick=${() => { listContentTypes(false); listContent(null); editContent(null); listChanges(true); showDiff(null); }}/>
                        <//>
                    </div>
                <//>
                <cloudy-ui-app>
                    <${ListContentTypes} renderIf=${listingContentTypes} activeContentType=${listingContent} onSelectContentType=${contentType => listContent(contentType)}/>
                    <${ListContentContextProvider} renderIf=${listingContent}>
                        <${ListContent} activeContentReference=${editingContent} contentType=${listingContent} onEditContent=${(contentReference, nameHint) => editContent(contentStateManager.getOrCreateStateForExistingContent(contentReference, nameHint))} onNewContent=${contentType => editContent(contentStateManager.createStateForNewContent(contentType))} onClose=${() => listContent(null)}/>
                    <//>
                    <${PendingChanges} renderIf=${listingChanges} onSelect=${contentReference => showDiff(contentReference)} onClose=${() => { listChanges(null); showDiff(null); editContent(null); listContentTypes(true); }}/>

                    ${listingChanges ? html`${showDiffBlade}${editContentBlade}` : html`${editContentBlade}${showDiffBlade}`}
                <//>
            <//>
        <//>
    `;
}

export default App;
