import html from '../util/html.js';
import { useContext, useEffect, useState } from '../lib/preact.hooks.module.js';
import showDiffContext from './show-diff-context.js';
import Blade from '../components/blade/blade.js';
import fieldModelBuilder from '../FormSupport/field-model-builder.js';
import contentGetter from '../data/content-getter.js';
import Diff from './lib/diff.js';
import changeTracker from '../edit-content/change-tracker.js';

function DiffField(props) {
    const { fieldModel, change, initialValue, value } = props;

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
            <${fieldModel.controlType} fieldModel=${fieldModel} initialValue=${value} readonly />
        <//>
    `;
}

function ShowDiff() {
    const [showingDiff] = useContext(showDiffContext);

    if (!showingDiff) {
        return null;
    }

    const [fieldModels, setFieldModels] = useState();

    useEffect(() => {
        fieldModelBuilder.getFieldModels(showingDiff.contentTypeId).then(fieldModels => setFieldModels(fieldModels));
    }, [showingDiff.contentTypeId]);

    if (!fieldModels) {
        return null;
    }

    const [content, setContent] = useState();

    useEffect(() => {
        showingDiff.contentId && contentGetter.get(showingDiff.contentId, showingDiff.contentTypeId).then(content => setContent(content));
    }, null);

    if (!content) {
        return null;
    }

    const changes = changeTracker.getFor(showingDiff.contentId, showingDiff.contentTypeId);

    return html`
        <${Blade} title=${'Pending changes' + (showingDiff.changedFields.length ? ` (${showingDiff.changedFields.length})` : '')}>
            <div class=cloudy-ui-form>
                ${fieldModels.map(fieldModel => html`<${DiffField}
                    change=${changes.changedFields.find(f => f.path[f.path.length - 1] == fieldModel.descriptor.id)}
                    initialValue=${content[fieldModel.descriptor.id]}
                    value=${changeTracker.getPendingValue(showingDiff.contentId, showingDiff.contentTypeId, [fieldModel.descriptor.id], content[fieldModel.descriptor.id])}
                    fieldModel=${fieldModel}
                />`)}
            <//>
        <//>
    `;
}

export default ShowDiff;