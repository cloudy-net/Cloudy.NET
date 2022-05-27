
import { useState } from '../lib/preact.hooks.module.js';
import { createRef } from '../lib/preact.module.js';

import html from '../util/html.js';
import Button from '../components/button/button.js'
import SimpleField from './simple-field.js';
import PopupMenu from '../components/popup-menu/popup-menu.js';
import ListItem from '../components/list/list-item.js';
import EmbeddedForm from './form/embedded-form.js';
import propertyGetter from '../data/property-getter.js';

const generateNewArrayElementKey = () => (Math.random() * 0xFFFFFF << 0).toString(16).padStart(6, '0'); // https://stackoverflow.com/questions/5092808/how-do-i-randomly-generate-html-hex-color-codes-using-javascript

export default function SortableField({ path, fieldModel, initialState }) {
    const ref = createRef(null);

    ;

    let [values, setValues] = useState(propertyGetter.get(initialState, path) || []);

    const add = type => {
        const key = generateNewArrayElementKey();
        const value = { type, key, value: {} };
        ref.current.dispatchEvent(new CustomEvent('cloudy-ui-form-change', { bubbles: true, detail: { change: { path, type: 'array', operation: 'add', value } } }));
        setValues([...values, value]);
    };

    const renderValue = value => {
        const elementPath = [...path, value.key];
        if (fieldModel.descriptor.embeddedFormId) {
            return html`<cloudy-ui-sortable-item-form>
                <${EmbeddedForm}
                    path=${elementPath}
                    formId=${formId}
                    initialState=${initialState}
                />
            <//>`;
        }

        if (fieldModel.descriptor.isPolymorphic && value.type) {
            return html`<cloudy-ui-sortable-item-form>
                <${EmbeddedForm}
                    path=${elementPath}
                    formId=${value.type}
                    initialState=${initialState}
                />
            <//>`;
        }
    
        return html`<${SimpleField}
            path=${elementPath}
            fieldModel=${fieldModel}
            initialState=${initialState}
        />`;
    };

    const addButton = () => {
        if (fieldModel.descriptor.embeddedFormId) {
            return html`<${EmbeddedForm}
                path=${path}
                formId=${formId}
                initialState=${initialState}
            />`;
        }

        if (fieldModel.descriptor.isPolymorphic) {
            return html`
                <${PopupMenu} text="Add" position="right">
                    ${fieldModel.descriptor.polymorphicCandidates.map(c => html`<${ListItem} text="${c}" onclick=${() => add(c)}/>`)}
                <//>
            `;
        }

        return html`<${Button} text="Add" onClick=${() => add()} />`;
    };

    return html`
        <div class="cloudy-ui-form-field" ref=${ref}>
            <div class="cloudy-ui-form-field-label">${fieldModel.descriptor.label || fieldModel.descriptor.id}</div>
            <cloudy-ui-sortable-items>
                ${values.map(value => renderValue(value))}
            <//>
            <cloudy-ui-sortable-buttons>
                ${addButton()}
            <//>
        </div>
    `;
}