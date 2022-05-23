import propertyGetter from '../../data/property-getter.js';
import { createRef } from '../../lib/preact.module.js';
import html from '../../util/html.js';

function PolymorphicForm({ fieldModel, initialState, path, onchange, readonly }) {
    const ref = onchange && createRef();
    const changeEvent = onchange && (event => onchange(ref.current, event.srcElement.value));

    return html`
        <div>
            Test
        <//>
    `;
}

export default PolymorphicForm;