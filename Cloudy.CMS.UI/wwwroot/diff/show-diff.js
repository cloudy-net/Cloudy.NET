import html from '../util/html.js';
import { useEffect, useState, useCallback } from '../lib/preact.hooks.module.js';
import Blade from '../components/blade/blade.js';
import fieldModelBuilder from '../FormSupport/field-model-builder.js';
import Diff from './lib/diff.js';
import stateManager from '../edit-content/state-manager.js';
import nameGetter from '../data/name-getter.js';
import contentTypeProvider from '../data/content-type-provider.js';

function DiffField({ fieldModel, change, initialValue, value }) {
    if (change && fieldModel.descriptor.control && (fieldModel.descriptor.control.id == 'text' || fieldModel.descriptor.control.id == 'textarea')) {
        return html`
            <div class="cloudy-ui-form-field cloudy-ui-simple">
                <div class="cloudy-ui-form-field-label">${fieldModel.descriptor.label || fieldModel.descriptor.id}<//>
                <div class=cloudy-ui-form-input>
                    ${Diff(initialValue || '', value || '', 0).map(([state, segment]) => html`<span class=${state == Diff.INSERT ? 'cloudy-ui-diff-insert' : state == Diff.DELETE ? 'cloudy-ui-diff-delete' : null}>${segment}</span>`)}
                <//>
            <//>
        `;
    }

    return html`
        <div class="cloudy-ui-form-field cloudy-ui-simple">
            <div class="cloudy-ui-form-field-label">${fieldModel.descriptor.label || fieldModel.descriptor.id}<//>
            <${fieldModel.controlType} fieldModel=${fieldModel} initialValue=${initialValue} />
        <//>
    `;
}

function ShowDiff({ renderIf, contentReference, onClose }) {
    if (!renderIf) {
        return;
    }

    const [fieldModels, setFieldModels] = useState();

    useEffect(() => {
        fieldModelBuilder.getFieldModels(contentReference.contentTypeId).then(fieldModels => setFieldModels(fieldModels));
    }, [contentReference]);

    if (!fieldModels) {
        return;
    }

    const undoChanges = useCallback(() => {
        if (confirm('Undo changes? This is not reversible')) {
            resetChange(diffData.contentId, diffData.contentTypeId);
            onClose();
        }
    }, []);

    const saveChange = useCallback(() => {
        applyFor(diffData.contentId, diffData.contentTypeId, () => {
            onClose();
        });
    }, []);

    const contentType = contentTypeProvider.get(contentReference.contentTypeId);
    const state = stateManager.getState(contentReference);

    const getPendingValue = key => {
        const changedField = state.changedFields.find(f => f.path.length == 1 && f.path[0] == key && f.type == 'simple' && f.operation == 'set');

        if (changedField) {
            return changedField.value;
        }

        return state.referenceValues[key];
    };

    return html`
        <${Blade} title=${nameGetter.getNameOfState(state, contentType)} onClose=${() => onClose()}>
            <cloudy-ui-blade-content>
                <div class="cloudy-ui-form">
                    ${fieldModels.map(fieldModel => html`<${DiffField}
                        change=${state.changedFields.find(f => f.path[f.path.length - 1] == fieldModel.descriptor.id)}
                        initialValue=${state.referenceValues[fieldModel.descriptor.id]}
                        value=${getPendingValue(fieldModel.descriptor.id)}
                        fieldModel=${fieldModel}
                    />`)}
                <//>         
            <//>
            <cloudy-ui-blade-footer>
                <cloudy-ui-button tabindex="0" style="margin-left: auto;" onclick=${() => undoChanges()}>Undo changes</cloudy-ui-button>
                <cloudy-ui-button tabindex="0" class="primary" style="margin-left: 10px;" onclick=${() => saveChange()}>Save</cloudy-ui-button>
            </cloudy-ui-blade-footer>
        <//>
    `;
}

export default ShowDiff;