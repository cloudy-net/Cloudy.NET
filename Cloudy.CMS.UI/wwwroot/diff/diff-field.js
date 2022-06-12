import html from '../util/html.js';
import diff from './lib/diff.js';

const buildDiff = ([state, segment]) => {
    if(state == diff.INSERT){
        return html`<span class=cloudy-ui-diff-insert>${segment}</span>`;
    }

    if(state == diff.DELETE){
        return html`<span class=cloudy-ui-diff-delete>${segment}</span>`;
    }

    return segment;
};

function DiffField({ fieldDescriptor, initialValue, value }) {
    let result = diff(initialValue || '', value || '', 0).map(buildDiff);

    const getChangeBadge = () => {
        if(!initialValue && value){
            return html`<cloudy-ui-form-field-change-badge class=cloudy-ui-added title="This value has been added."><//>`;
        }

        if(initialValue && !value){
            return html`<cloudy-ui-form-field-change-badge class=cloudy-ui-removed title="This value has been removed."><//>`;
        }

        if(initialValue != value){
            return html`<cloudy-ui-form-field-change-badge class=cloudy-ui-modified title="This value has been modified."><//>`;
        }
    };

    return html`
        <div class="cloudy-ui-form-field cloudy-ui-simple cloudy-ui-readonly">
            <div class="cloudy-ui-form-field-label">${fieldDescriptor.label || fieldDescriptor.id}${getChangeBadge()}<//>
            <div class=cloudy-ui-form-input>
                ${result == '' ? html`<br/>` : result}
            <//>
        <//>
    `;
}

export default DiffField;