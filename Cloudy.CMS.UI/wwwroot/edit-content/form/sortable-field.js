
import { useContext, useState } from '../../lib/preact.hooks.module.js';
import html from '../../util/html.js';
import Button from '../../components/button/button.js'
import SimpleField from './simple-field.js';
import PopupMenu from '../../components/popup-menu/popup-menu.js';
import ListItem from '../../components/list/list-item.js';
import EmbeddedForm from './embedded-form.js';
import getValue from '../../util/get-value.js';
import stateManager from '../state-manager.js';
import contentTypeContext from '../../list-content-types/content-type-context.js';

const generateNewArrayElementKey = () => (Math.random() * 0xFFFFFF << 0).toString(16).padStart(6, '0'); // https://stackoverflow.com/questions/5092808/how-do-i-randomly-generate-html-hex-color-codes-using-javascript

const renderElement = (path, fieldDescriptor, state, element) => {
    const elementPath = [...path, element.key];
    if (fieldDescriptor.embeddedFormId) {
        return html`<cloudy-ui-sortable-item-form>
            <${EmbeddedForm}
                path=${elementPath}
                formId=${formId}
                state=${state}
            />
        <//>`;
    }

    if (fieldDescriptor.isPolymorphic && element.type) {
        return html`<cloudy-ui-sortable-item-form>
            <${EmbeddedForm}
                path=${elementPath}
                formId=${element.type}
                state=${state}
            />
        <//>`;
    }

    return html`<${SimpleField}
        path=${elementPath}
        fieldDescriptor=${fieldDescriptor}
        state=${state}
    />`;
};

const AddButton = ({ path, fieldDescriptor, state, onAdd }) => {
    const contentTypes = useContext(contentTypeContext);

    if (fieldDescriptor.embeddedFormId) {
        return html`<${EmbeddedForm}
            path=${path}
            formId=${formId}
            state=${state}
        />`;
    }

    if (fieldDescriptor.isPolymorphic) {
        return html`
            <${PopupMenu} text="Add" position="right">
                ${fieldDescriptor.polymorphicCandidates.map(c => html`<${ListItem} text="${contentTypes[c].name}" onclick=${() => onAdd(c)}/>`)}
            <//>
        `;
    }

    return html`<${Button} text="Add" onClick=${() => onAdd()} />`;
};

export default function SortableField({ path, fieldDescriptor, state }) {
    let [elements, setElements] = useState((getValue(state.referenceValues, path) || []).map((value, index) => ({ key: index, type: value.type })));

    const onAdd = type => {
        const element = { key: generateNewArrayElementKey(), type };
        stateManager.registerArrayAdd(state.contentReference, path, element);
        setElements([...elements, element]);
    };

    return html`
        <div class="cloudy-ui-form-field">
            <div class="cloudy-ui-form-field-label">${fieldDescriptor.label || fieldDescriptor.id}</div>
            <cloudy-ui-sortable-items>
                ${elements.map(value => renderElement(path, fieldDescriptor, state, value))}
            <//>
            <cloudy-ui-sortable-buttons>
                <${AddButton} path=${path} fieldDescriptor=${fieldDescriptor} state=${state} onAdd=${onAdd} />
            <//>
        </div>
    `;
}