import html from '../util/html.js';
import { useCallback, useContext } from '../lib/preact.hooks.module.js';
import Blade from '../components/blade/blade.js';
import diff from './lib/diff.js';
import contentTypeProvider from '../data/content-type-provider.js';
import ReviewRemoteChangesContext from './review-remote-changes-context.js';
import stateManager from '../edit-content/state-manager.js';
import fieldModelContext from '../edit-content/form/field-model-context.js';

const buildDiff = ([state, segment]) => {
    if(state == diff.INSERT){
        return html`<span class=cloudy-ui-diff-insert>${segment}</span>`;
    }

    if(state == diff.DELETE){
        return html`<span class=cloudy-ui-diff-delete>${segment}</span>`;
    }

    return segment;
};

function DiffField({ fieldModel, initialValue, value }) {
    // fieldModel.descriptor.control.id == 'text'

    let result = diff(initialValue || '', value || '', 0).map(buildDiff);
    return html`
        <div class="cloudy-ui-form-field cloudy-ui-simple cloudy-ui-readonly">
            <div class="cloudy-ui-form-field-label">${fieldModel.descriptor.label || fieldModel.descriptor.id}<//>
            <div class=cloudy-ui-form-input>
                ${result}
            <//>
        <//>
    `;
}

function ReviewRemoteChanges({ contentReference, onClose }) {
    const fieldModels = useContext(fieldModelContext)[contentReference.contentTypeId];
    
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
                    ${fieldModels.map(fieldModel => html`<${DiffField}
                        initialValue=${state.referenceValues[fieldModel.descriptor.id]}
                        value=${state.newVersion.referenceValues[fieldModel.descriptor.id]}
                        fieldModel=${fieldModel}
                    />`)}
                <//>
            <//>
            <cloudy-ui-blade-footer>
                <cloudy-ui-button tabindex="0" class="primary" onclick=${() => discard()}>Discard</cloudy-ui-button>
            </cloudy-ui-blade-footer>
        <//>
    `;
}

export default ReviewRemoteChanges;