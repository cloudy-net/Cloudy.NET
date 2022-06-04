import html from './util/html.js';
import ListContentTypes from './list-content-types/list-content-types.js';
import ListContent from './list-content-types/list-content.js';
import EditContent from './edit-content/edit-content.js';
import TotalChangesContextProvider from './edit-content/total-changes-context-provider.js';
import PopupMenuContextProvider from './components/popup-menu/popup-menu-context-provider.js';
import TotalChangesButton from './diff/total-changes-button.js';
import { useState } from './lib/preact.hooks.module.js';
import stateManager from './edit-content/state-manager.js';
import StateContextProvider from './edit-content/state-context-provider.js';
import PendingChanges from './diff/pending-changes.js';
import ShowDiff from './diff/show-diff.js';
import ShowDiffContextProvider from './diff/show-diff-context-provider.js';
import ReviewRemoteChanges from './diff/review-remote-changes.js';
import ReviewRemoteChangesContextProvider from './diff/review-remote-changes-context-provider.js';
import ListContentContextProvider from './list-content-types/list-content-context-provider.js';
import FieldModelContextProvider from './edit-content/form/field-model-context-provider.js';
import ContentTypeContextProvider from './list-content-types/content-type-context-provider.js';

function App({ title }) {
    const [listingContentTypes, listContentTypes] = useState(true);
    const [listingContent, listContent] = useState(null);
    const [editingContent, editContent] = useState(null);
    const [listingChanges, listChanges] = useState(false);
    const [showingDiff, showDiff] = useState(null);
    const [reviewingRemoteChanges, reviewRemoteChanges] = useState(false);

    const editContentBlade = html`
        <${StateContextProvider} renderIf=${editingContent} contentReference=${editingContent}>
            <${EditContent} contentReference=${editingContent} onClose=${() => { editContent(null); listingContent ? showDiff(null) : null; }} canDiff=${listingContent} onDiff=${() => showDiff(editingContent)} reviewRemoteChanges=${() => { reviewRemoteChanges(editingContent); }}/>
        <//>
    `;

    const showDiffBlade = html`
        <${ShowDiffContextProvider} renderIf=${showingDiff} contentReference=${showingDiff}>
            <${ShowDiff} contentReference=${showingDiff} onClose=${() => { showDiff(null); listingChanges ? editContent(null) : null }} canEdit=${listingChanges} onEdit=${() => editContent(showingDiff)} onSave=${() => { stateManager.save([showingDiff]); }}/>
        <//>
    `;

    return html`
        <${ContentTypeContextProvider}>
            <${FieldModelContextProvider}>
                <${PopupMenuContextProvider}>
                    <cloudy-ui-portal>
                        <cloudy-ui-portal-nav>
                            <cloudy-ui-portal-nav-title>${title}<//>
                            <div>
                                <${TotalChangesContextProvider}>
                                    <${TotalChangesButton} onClick=${() => { listContent(null); editContent(null); showDiff(null); if(!listingChanges) { listContentTypes(false); listChanges(true); } else { listContentTypes(true); listChanges(false); } }}/>
                                <//>
                            </div>
                        <//>
                        <cloudy-ui-app>
                            <${ListContentTypes} renderIf=${listingContentTypes} activeContentType=${listingContent} onSelectContentType=${contentType => listContent(contentType)}/>
                            <${ListContentContextProvider} renderIf=${listingContent}>
                                <${ListContent} activeContentReference=${editingContent} contentType=${listingContent} onEditContent=${(contentReference, nameHint) => { stateManager.createOrUpdateStateForExistingContent(contentReference, nameHint); editContent(contentReference); showDiff(null); }} onNewContent=${contentType => { const state = stateManager.createStateForNewContent(contentType); editContent(state.contentReference); }} onClose=${() => listContent(null)}/>
                            <//>
                            <${PendingChanges} renderIf=${listingChanges} onSelect=${contentReference => showDiff(contentReference)} onClose=${() => { listChanges(null); showDiff(null); editContent(null); listContentTypes(true); }}/>

                            ${listingChanges ? html`${showDiffBlade}${editContentBlade}` : html`${editContentBlade}${showDiffBlade}`}

                            <${ReviewRemoteChangesContextProvider} renderIf=${reviewingRemoteChanges} contentReference=${reviewingRemoteChanges}>
                                <${ReviewRemoteChanges} contentReference=${reviewingRemoteChanges} onClose=${() => { reviewRemoteChanges(null); }} />
                            <//>
                        <//>
                    <//>
                <//>
            <//>
        <//>
    `;
}

export default App;
