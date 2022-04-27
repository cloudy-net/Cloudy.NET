import html from '../util/html.js';
import { useContext, useEffect, useState, useCallback } from '../lib/preact.hooks.module.js';
import showDiffContext from './show-diff-context.js';
import Blade from '../components/blade/blade.js';
import fieldModelBuilder from '../FormSupport/field-model-builder.js';
import contentGetter from '../data/content-getter.js';
import Diff from './lib/diff.js';
import pendingChangesContext from './pending-changes-context.js';

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

function ShowDiff({ renderIf }) {
    if (!renderIf) {
        return;
    }

    const [fieldModels, setFieldModels] = useState();

    if (!diffData) {
        return null;
    }

    useEffect(() => {
        fieldModelBuilder.getFieldModels(diffData.contentTypeId).then(fieldModels => setFieldModels(fieldModels));
    }, [diffData.contentTypeId]);

    if (!fieldModels) {
        return null;
    }

    useEffect(() => {
        setChanges(getFor(diffData.contentId, diffData.contentTypeId));
        diffData.contentId && contentGetter.get([diffData.contentId], diffData.contentTypeId).then(content => setContent(content));
    }, [diffData.contentId, diffData.contentTypeId]);

    if (!content) {
        return null;
    }

    const undoChanges = useCallback(() => {
        if (confirm('Undo changes? This is not reversible')) {
            resetChange(diffData.contentId, diffData.contentTypeId);
            setDiffData(null);
        }
    }, [diffData]);

    const saveChange = useCallback(() => {
        applyFor(diffData.contentId, diffData.contentTypeId, () => {
            setDiffData(null);
        });
    }, [diffData, applyFor]);

    return html`
        <${Blade} title=${'Pending changes' + (diffData?.changedFields?.length ? `(${diffData.changedFields.length})` : '')} onclose=${() => setDiffData(null)}>
            <cloudy-ui-blade-content>
                <div class="cloudy-ui-form">
                    ${fieldModels.map(fieldModel => html`<${DiffField}
                        change=${changes?.changedFields?.find(f => f.path[f.path.length - 1] == fieldModel.descriptor.id)}
                        initialValue=${getPendingValue(diffData.contentId, diffData.contentTypeId, [fieldModel.descriptor.id], content[fieldModel.descriptor.id])}
                        value=${getPendingValue(diffData.contentId, diffData.contentTypeId, [fieldModel.descriptor.id], content[fieldModel.descriptor.id])}
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