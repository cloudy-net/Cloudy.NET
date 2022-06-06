import html from './util/html.js';
import ListContentTypes from './list-content-types/list-content-types.js';
import ListContent from './list-content-types/list-content.js';
import EditContent from './edit-content/edit-content.js';
import PopupMenuContextProvider from './components/popup-menu/popup-menu-context-provider.js';
import TotalChangesButton from './diff/total-changes-button.js';
import { useState } from './lib/preact.hooks.module.js';
import stateManager from './edit-content/state-manager.js';
import StateContextProvider from './edit-content/state-context-provider.js';
import PendingChanges from './diff/pending-changes.js';
import ReviewChanges from './diff/review-changes.js';
import ReviewChangesContextProvider from './diff/review-changes-context-provider.js';
import ReviewRemoteChanges from './diff/review-remote-changes.js';
import ReviewRemoteChangesContextProvider from './diff/review-remote-changes-context-provider.js';
import FieldDescriptorContextProvider from './edit-content/form/field-descriptor-context-provider.js';
import ContentTypeContextProvider from './list-content-types/content-type-context-provider.js';
import FormControlContextProvider from './edit-content/form/field-control-context-provider.js';

function App({ title }) {
    const [listingContentTypes, listContentTypes] = useState(true);
    const [listingContent, listContent] = useState(null);
    const [editingContent, editContent] = useState(null);
    const [listingChanges, listChanges] = useState(false);
    const [reviewingChanges, reviewChanges] = useState(null);
    const [reviewingRemoteChanges, reviewRemoteChanges] = useState(false);

    const editContentBlade = html`
        <${StateContextProvider} renderIf=${editingContent} contentReference=${editingContent}>
            <${EditContent} contentReference=${editingContent} onClose=${() => { editContent(null); listingContent ? reviewChanges(null) : null; }} canDiff=${listingContent} onDiff=${() => reviewChanges(editingContent)} reviewRemoteChanges=${() => { reviewRemoteChanges(editingContent); }}/>
        <//>
    `;

    const reviewChangesBlade = html`
        <${ReviewChangesContextProvider} renderIf=${reviewingChanges} contentReference=${reviewingChanges}>
            <${ReviewChanges} contentReference=${reviewingChanges} onClose=${() => { reviewChanges(null); listingChanges ? editContent(null) : null }} canEdit=${listingChanges} onEdit=${() => editContent(reviewingChanges)} onSave=${() => { stateManager.save([reviewingChanges]); }}/>
        <//>
    `;

    return html`
        <${ContentTypeContextProvider}>
            <${FieldDescriptorContextProvider}>
                <${FormControlContextProvider}>
                    <${PopupMenuContextProvider}>
                        <cloudy-ui-portal>
                            <cloudy-ui-portal-nav>
                                <cloudy-ui-portal-nav-title>${title}<//>
                                <div>
                                    <${TotalChangesButton} onClick=${() => { listContent(null); editContent(null); reviewChanges(null); if(!listingChanges) { listContentTypes(false); listChanges(true); } else { listContentTypes(true); listChanges(false); } }}/>
                                </div>
                            <//>
                            <cloudy-ui-app>
                                <${ListContentTypes} renderIf=${listingContentTypes} activeContentType=${listingContent} onSelectContentType=${contentType => listContent(contentType)}/>
                                <${ListContent} renderIf=${listingContent} activeContentReference=${editingContent} contentType=${listingContent} onEditContent=${(contentReference, nameHint) => { stateManager.createOrUpdateStateForExistingContent(contentReference, nameHint); editContent(contentReference); reviewChanges(null); }} onNewContent=${contentType => { const state = stateManager.createStateForNewContent(contentType); editContent(state.contentReference); }} onClose=${() => listContent(null)}/>
                                <${PendingChanges} renderIf=${listingChanges} onSelect=${contentReference => reviewChanges(contentReference)} onClose=${() => { listChanges(null); reviewChanges(null); editContent(null); listContentTypes(true); }}/>

                                ${listingChanges ? html`${reviewChangesBlade}${editContentBlade}` : html`${editContentBlade}${reviewChangesBlade}`}

                                <${ReviewRemoteChangesContextProvider} renderIf=${reviewingRemoteChanges} contentReference=${reviewingRemoteChanges}>
                                    <${ReviewRemoteChanges} contentReference=${reviewingRemoteChanges} onClose=${() => { reviewRemoteChanges(null); }} />
                                <//>
                            <//>
                        <//>
                    <//>
                <//>
            <//>
        <//>
    `;
}

export default App;
