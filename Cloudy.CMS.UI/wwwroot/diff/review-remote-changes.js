import html from '../util/html.js';
import { useCallback, useContext } from '../lib/preact.hooks.module.js';
import Blade from '../components/blade/blade.js';
import ReviewRemoteChangesContext from './review-remote-changes-context.js';
import stateManager from '../edit-content/state-manager.js';
import DiffField from './diff-field.js';
import contentTypeProvider from '../list-content-types/content-type-provider.js';
import fieldDescriptorProvider from '../edit-content/form/field-descriptor-provider.js';

function renderDiffField(fieldDescriptor, initialValue, value) {
    if(fieldDescriptor.embeddedFormId){
        const fieldDescriptors = fieldDescriptorProvider(fieldDescriptor.embeddedFormId);

        const state = initialValue && !value ? 'cloudy-ui-diff-delete' :
        !initialValue && value ? 'cloudy-ui-diff-insert' :
        '';
        
        return html`<fieldset class="cloudy-ui-form-field ${state}">
            <legend class="cloudy-ui-form-field-label">${fieldDescriptor.label || fieldDescriptor.id}<//>
            ${fieldDescriptors.map(f => renderDiffField(f, initialValue ? initialValue[f.id] : null, value ? value[f.id] : null))}
        <//>`;
    }

    return html`<${DiffField}
        fieldDescriptor=${fieldDescriptor}
        initialValue=${initialValue}
        value=${value}
    />`;
}

function ReviewRemoteChanges({ contentReference, onClose }) {
    const fieldDescriptors = fieldDescriptorProvider.get(contentReference.contentTypeId);
    const contentType = contentTypeProvider.get(contentReference.contentTypeId);
    const state = useContext(ReviewRemoteChangesContext);
    
    const discard = useCallback(() => {
        stateManager.discardNewVersion(contentReference);
        onClose();
    }, []);

    return html`
        <${Blade} scrollIntoView=${contentReference} title="Review remote changes" onClose=${() => onClose()}>
            <cloudy-ui-blade-content>
                <cloudy-ui-info-message>
                    These are the changes that were done after the ${contentType.lowerCaseName} was first retrieved.
                <//>
                <div class="cloudy-ui-form">
                    ${fieldDescriptors.map(fieldDescriptor => renderDiffField(fieldDescriptor, state.referenceValues[fieldDescriptor.id], state.newVersion.referenceValues[fieldDescriptor.id]))}
                <//>
            <//>
            <cloudy-ui-blade-footer>
                <cloudy-ui-button tabindex="0" class="primary" onclick=${() => discard()}>Discard</cloudy-ui-button>
            </cloudy-ui-blade-footer>
        <//>
    `;
}

export default ReviewRemoteChanges;