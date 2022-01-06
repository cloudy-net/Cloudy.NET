import html from '../util/html.js';
import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';
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

function ShowDiff() {
    const [diffData] = useContext(showDiffContext);
    const [content, setContent] = useState();
    const [fieldModels, setFieldModels] = useState();
    const [, , , getPendingValue, getFor] = useContext(pendingChangesContext);
    const [changes, setChanges] = useState();

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

    return html`
        <${Blade} title=${'Pending changes' + (diffData?.changedFields?.length ? `(${diffData.changedFields.length})` : '')}>
            <div class=cloudy-ui-form>
                ${fieldModels.map(fieldModel => html`<${DiffField}
                    change=${changes.changedFields.find(f => f.path[f.path.length - 1] == fieldModel.descriptor.id)}
                    initialValue=${content[fieldModel.descriptor.id]}
                    value=${getPendingValue(diffData.contentId, diffData.contentTypeId, [fieldModel.descriptor.id], content[fieldModel.descriptor.id])}
                    fieldModel=${fieldModel}
                />`)}
            <//>
        <//>
    `;
}

export default ShowDiff;