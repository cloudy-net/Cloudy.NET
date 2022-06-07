import html from '../util/html.js';
import { useCallback, useContext } from '../lib/preact.hooks.module.js';
import Blade from '../components/blade/blade.js';
import nameGetter from '../data/name-getter.js';
import reviewChangesContext from './review-changes-context.js';
import ContextMenu from '../components/context-menu/context-menu.js';
import ListItem from '../components/list/list-item.js';
import stateManager from '../edit-content/state-manager.js';
import getIntermediateSimpleValue from '../util/get-intermediate-simple-value.js';
import DiffField from './diff-field.js';
import contentTypeProvider from '../list-content-types/content-type-provider.js';
import fieldDescriptorProvider from '../edit-content/form/field-descriptor-provider.js';
import getReferenceValue from '../util/get-reference-value.js';

function renderDiffField(fieldDescriptor, state, path){
    if(fieldDescriptor.embeddedFormId){
        const fieldDescriptors = fieldDescriptorProvider.get(fieldDescriptor.embeddedFormId);
        
        return html`<fieldset class="cloudy-ui-form-field">
            <legend class="cloudy-ui-form-field-label">${fieldDescriptor.label || fieldDescriptor.id}<//>
            ${fieldDescriptors.map(f => renderDiffField(f, state, [...path, f.id]))}
        <//>`;
    }

    return html`<${DiffField}
        fieldDescriptor=${fieldDescriptor}
        initialValue=${getReferenceValue(state, path)}
        value=${getIntermediateSimpleValue(state, path)}
    />`;
}

function ReviewChanges({ contentReference, onClose, canEdit, onEdit }) {
    const fieldDescriptors = fieldDescriptorProvider.get(contentReference.contentTypeId);
    const contentType = contentTypeProvider.get(contentReference.contentTypeId);
    const state = useContext(reviewChangesContext);

    const saveButton = html`<cloudy-ui-button tabindex="0" class="primary" onclick=${() => stateManager.save([contentReference])} disabled=${!state.simpleChanges.length}>Save</cloudy-ui-button>`;
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
                ${saveButton}
            </cloudy-ui-blade-footer>
        <//>
    `;
}

export default ReviewChanges;