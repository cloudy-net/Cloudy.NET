import html from '../util/html.js';
import { useEffect, useState, useCallback, useContext } from '../lib/preact.hooks.module.js';
import Blade from '../components/blade/blade.js';
import diff from './lib/diff.js';
import nameGetter from '../data/name-getter.js';
import contentTypeProvider from '../data/content-type-provider.js';
import showDiffContext from './show-diff-context.js';
import ContextMenu from '../components/context-menu/context-menu.js';
import ListItem from '../components/list/list-item.js';
import stateManager from '../edit-content/state-manager.js';
import fieldModelContext from '../edit-content/form/field-model-context.js';
import arrayEquals from '../util/array-equals.js';
import getValueFromObject from '../util/get-value.js';
import getIntermediateValue from '../util/get-intermediate-value.js';

const buildDiff = ([state, segment]) => {
    if(state == diff.INSERT){
        return html`<span class=cloudy-ui-diff-insert>${segment}</span>`;
    }

    if(state == diff.DELETE){
        return html`<span class=cloudy-ui-diff-delete>${segment}</span>`;
    }

    return segment;
};

function DiffField({ fieldModel, change, initialValue, value }) {
    let result = diff(initialValue || '', value || '', 0).map(buildDiff);
    return html`
        <div class="cloudy-ui-form-field cloudy-ui-simple cloudy-ui-readonly">
            <div class="cloudy-ui-form-field-label">${fieldModel.descriptor.label || fieldModel.descriptor.id}<//>
            <div class=cloudy-ui-form-input>
                ${result == '' ? html`<br/>` : result}
            <//>
        <//>
    `;
}

const getPendingValue = (state, path) => {
    const change = state.changes.find(f => arrayEquals(f.path, path) && f.type == 'simple' && f.operation == 'set');

    if (change) {
        return change.value;
    }

    return getValueFromObject(state.referenceValues, path);
};

function renderDiffField(fieldModel, state, path){
    if(fieldModel.descriptor.embeddedFormId){
        const fieldModels = useContext(fieldModelContext)[fieldModel.descriptor.embeddedFormId];
        
        return html`<fieldset class="cloudy-ui-form-field">
            <legend class="cloudy-ui-form-field-label">${fieldModel.descriptor.label || fieldModel.descriptor.id}<//>
            ${fieldModels.map(f => renderDiffField(f, state, [...path, f.descriptor.id]))}
        <//>`;
    }

    return html`<${DiffField}
        change=${state.changes.find(f => arrayEquals(f.path, path))}
        initialValue=${state.referenceValues[fieldModel.descriptor.id]}
        value=${getIntermediateValue(state.referenceValues, path, state.changes)}
        fieldModel=${fieldModel}
    />`;
}

function ShowDiff({ contentReference, onClose, canEdit, onEdit, onSave }) {
    const fieldModels = useContext(fieldModelContext)[contentReference.contentTypeId];

    const undoChanges = useCallback(() => {
        if (confirm('Undo changes? This is not reversible')) {
            resetChange(diffData.contentId, diffData.contentTypeId);
            onClose();
        }
    }, []);

    const contentType = contentTypeProvider.get(contentReference.contentTypeId);
    const state = useContext(showDiffContext);

    const save = async () => {
        onSave();
    }

    const editButton = canEdit ? html`<cloudy-ui-button tabindex="0" onclick=${() => onEdit()}>Edit</cloudy-ui-button>` : null;

    return html`
        <${Blade} scrollIntoView=${contentReference} title=${'Review ' + nameGetter.getNameOf(state.referenceValues, contentType)} onClose=${() => onClose()}>
            <cloudy-ui-blade-content>
                <div class="cloudy-ui-form">
                    ${fieldModels.map(fieldModel => renderDiffField(fieldModel, state, [fieldModel.descriptor.id]))}
                <//>
            <//>
            <cloudy-ui-blade-footer>
                <${ContextMenu} position="bottom">
                    <${ListItem} text="Discard changes" onclick=${() => { stateManager.discardChanges(contentReference); onClose(); }}/>
                <//>
                ${editButton}
                <cloudy-ui-button tabindex="0" class="primary" onclick=${() => save()} disabled=${!state.changes.length}>Save</cloudy-ui-button>
            </cloudy-ui-blade-footer>
        <//>
    `;
}

export default ShowDiff;