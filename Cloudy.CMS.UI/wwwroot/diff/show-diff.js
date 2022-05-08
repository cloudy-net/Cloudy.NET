import html from '../util/html.js';
import { useEffect, useState, useCallback, useContext } from '../lib/preact.hooks.module.js';
import Blade from '../components/blade/blade.js';
import fieldModelBuilder from '../FormSupport/field-model-builder.js';
import diff from './lib/diff.js';
import nameGetter from '../data/name-getter.js';
import contentTypeProvider from '../data/content-type-provider.js';
import showDiffContext from './show-diff-context.js';
import ContextMenu from '../components/context-menu/context-menu.js';
import ListItem from '../components/list/list-item.js';
import stateManager from '../edit-content/state-manager.js';

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

function ShowDiff({ contentReference, onClose, canEdit, onEdit, onSave }) {
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

    const contentType = contentTypeProvider.get(contentReference.contentTypeId);
    const state = useContext(showDiffContext);

    const getPendingValue = key => {
        const changedField = state.changedFields.find(f => f.path.length == 1 && f.path[0] == key && f.type == 'simple' && f.operation == 'set');

        if (changedField) {
            return changedField.value;
        }

        return state.referenceValues[key];
    };

    const save = async () => {
        onSave();
    }

    const editButton = canEdit ? html`<cloudy-ui-button tabindex="0" onclick=${() => onEdit()}>Edit</cloudy-ui-button>` : null;

    return html`
        <${Blade} scrollIntoView=${contentReference} title=${'Review ' + nameGetter.getNameOf(state.referenceValues, contentType)} onClose=${() => onClose()}>
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
                <${ContextMenu}>
                    <${ListItem} text="Discard changes" onclick=${() => stateManager.discardChanges(contentReference)}/>
                <//>
                ${editButton}
                <cloudy-ui-button tabindex="0" class="primary" onclick=${() => save()} disabled=${!state.changedFields.length}>Save</cloudy-ui-button>
            </cloudy-ui-blade-footer>
        <//>
    `;
}

export default ShowDiff;