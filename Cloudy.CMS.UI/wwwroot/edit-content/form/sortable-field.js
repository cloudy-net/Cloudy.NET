
import { useEffect, useState } from '../../lib/preact.hooks.module.js';
import html from '../../util/html.js';
import Button from '../../components/button/button.js'
import SimpleField from './simple-field.js';
import PopupMenu from '../../components/popup-menu/popup-menu.js';
import ListItem from '../../components/list/list-item.js';
import EmbeddedForm from './embedded-form.js';
import getReferenceValue from '../../util/get-reference-value.js';
import stateManager from '../state-manager.js';
import contentTypeProvider from '../../list-content-types/content-type-provider.js';
import arrayEquals from '../../util/array-equals.js';

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
                ${fieldDescriptor.polymorphicCandidates.map(c => html`<${ListItem} text="${contentTypeProvider.get(c).name}" onclick=${() => onAdd(c)}/>`)}
            <//>
        `;
    }

    return html`<${Button} text="Add" onClick=${() => onAdd()} />`;
};

export default function SortableField({ path, fieldDescriptor, state }) {
    let [elements, setElements] = useState([]);

    useEffect(() => {
        const referenceValue = getReferenceValue(state, path) || [];
        const arrayChanges = state.arrayChanges.filter(add => arrayEquals(add.path, path));
        
        setElements([
            ...referenceValue.map((value, index) => ({ key: index, type: value.type })),
            ...arrayChanges.map(({ key, type }) => ({ key, type })),
        ]);
    }, [path, state]);

    const onAdd = type => {
        const element = { key: generateNewArrayElementKey(), type };
        stateManager.registerArrayAdd(state.contentReference, path, element.key, element.type);
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