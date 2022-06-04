import html from '../util/html.js';
import { useEffect, useState, useCallback, useContext } from '../lib/preact.hooks.module.js';
import Blade from '../components/blade/blade.js';
import diff from './lib/diff.js';
import nameGetter from '../data/name-getter.js';
import showDiffContext from './show-diff-context.js';
import ContextMenu from '../components/context-menu/context-menu.js';
import ListItem from '../components/list/list-item.js';
import stateManager from '../edit-content/state-manager.js';
import fieldDescriptorContext from '../edit-content/form/field-descriptor-context.js';
import arrayEquals from '../util/array-equals.js';
import getValue from '../util/get-value.js';
import getIntermediateSimpleValue from '../util/get-intermediate-simple-value.js';
import contentTypeContext from '../list-content-types/content-type-context.js';

const buildDiff = ([state, segment]) => {
    if(state == diff.INSERT){
        return html`<span class=cloudy-ui-diff-insert>${segment}</span>`;
    }

    if(state == diff.DELETE){
        return html`<span class=cloudy-ui-diff-delete>${segment}</span>`;
    }

    return segment;
};

function DiffField({ fieldDescriptor, change, initialValue, value }) {
    let result = diff(initialValue || '', value || '', 0).map(buildDiff);
    return html`
        <div class="cloudy-ui-form-field cloudy-ui-simple cloudy-ui-readonly">
            <div class="cloudy-ui-form-field-label">${fieldDescriptor.label || fieldDescriptor.id}<//>
            <div class=cloudy-ui-form-input>
                ${result == '' ? html`<br/>` : result}
            <//>
        <//>
    `;
}

function renderDiffField(fieldDescriptor, state, path){
    if(fieldDescriptor.embeddedFormId){
        const fieldDescriptors = useContext(fieldDescriptorContext)[fieldDescriptor.embeddedFormId];
        
        return html`<fieldset class="cloudy-ui-form-field">
            <legend class="cloudy-ui-form-field-label">${fieldDescriptor.label || fieldDescriptor.id}<//>
            ${fieldDescriptors.map(f => renderDiffField(f, state, [...path, f.descriptor.id]))}
        <//>`;
    }

    return html`<${DiffField}
        change=${state.simpleChanges.find(f => arrayEquals(f.path, path))}
        initialValue=${state.referenceValues[fieldDescriptor.id]}
        value=${getIntermediateSimpleValue(state.referenceValues, path, state.simpleChanges)}
        fieldDescriptor=${fieldDescriptor}
    />`;
}

function ShowDiff({ contentReference, onClose, canEdit, onEdit, onSave }) {
    const fieldDescriptors = useContext(fieldDescriptorContext)[contentReference.contentTypeId];

    const undoChanges = useCallback(() => {
        if (confirm('Undo changes? This is not reversible')) {
            resetChange(diffData.contentId, diffData.contentTypeId);
            onClose();
        }
    }, []);

    const contentType = useContext(contentTypeContext)[contentReference.contentTypeId];
    const state = useContext(showDiffContext);

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

export default ShowDiff;