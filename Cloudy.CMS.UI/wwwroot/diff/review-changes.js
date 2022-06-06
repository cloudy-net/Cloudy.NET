import html from '../util/html.js';
import { useCallback, useContext } from '../lib/preact.hooks.module.js';
import Blade from '../components/blade/blade.js';
import nameGetter from '../data/name-getter.js';
import reviewChangesContext from './review-changes-context.js';
import ContextMenu from '../components/context-menu/context-menu.js';
import ListItem from '../components/list/list-item.js';
import stateManager from '../edit-content/state-manager.js';
import fieldDescriptorContext from '../edit-content/form/field-descriptor-context.js';
import getIntermediateSimpleValue from '../util/get-intermediate-simple-value.js';
import contentTypeContext from '../list-content-types/content-type-context.js';
import DiffField from './diff-field.js';

function renderDiffField(fieldDescriptor, state, path){
    if(fieldDescriptor.embeddedFormId){
        const fieldDescriptors = useContext(fieldDescriptorContext)[fieldDescriptor.embeddedFormId];
        
        return html`<fieldset class="cloudy-ui-form-field">
            <legend class="cloudy-ui-form-field-label">${fieldDescriptor.label || fieldDescriptor.id}<//>
            ${fieldDescriptors.map(f => renderDiffField(f, state, [...path, f.id]))}
        <//>`;
    }

    return html`<${DiffField}
        fieldDescriptor=${fieldDescriptor}
        initialValue=${state.referenceValues[fieldDescriptor.id]}
        value=${getIntermediateSimpleValue(state, path)}
    />`;
}

function ReviewChanges({ contentReference, onClose, canEdit, onEdit, onSave }) {
    const fieldDescriptors = useContext(fieldDescriptorContext)[contentReference.contentTypeId];

    const undoChanges = useCallback(() => {
        if (confirm('Undo changes? This is not reversible')) {
            resetChange(diffData.contentId, diffData.contentTypeId);
            onClose();
        }
    }, []);

    const contentType = useContext(contentTypeContext)[contentReference.contentTypeId];
    const state = useContext(reviewChangesContext);

    const save = async () => {
        onSave();
    }

    const editButton = canEdit ? html`<cloudy-ui-button tabindex="0" onclick=${() => onEdit()}>Edit</cloudy-ui-button>` : null;

    return html`
        <${Blade} scrollIntoView=${contentReference} title=${'Review ' + nameGetter.getNameOf(state.referenceValues, contentType)} onClose=${() => onClose()}>
            <cloudy-ui-blade-content>
                <div class="cloudy-ui-form">
                    ${fieldDescriptors.map(fieldDescriptor => renderDiffField(fieldDescriptor, state, [fieldDescriptor.id]))}
                <//>
            <//>
            <cloudy-ui-blade-footer>
                <${ContextMenu} position="bottom">
                    <${ListItem} text="Discard changes" onclick=${() => { stateManager.discardChanges(contentReference); onClose(); }}/>
                <//>
                ${editButton}
                <cloudy-ui-button tabindex="0" class="primary" onclick=${() => save()} disabled=${!state.simpleChanges.length}>Save</cloudy-ui-button>
            </cloudy-ui-blade-footer>
        <//>
    `;
}

export default ReviewChanges;