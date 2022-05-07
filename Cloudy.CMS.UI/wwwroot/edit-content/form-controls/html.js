import { useEffect } from '../../lib/preact.hooks.module.js';
import { createRef } from '../../lib/preact.module.js';
import html from '../../util/html.js';

function Html({ fieldModel, initialValue, onchange, readonly }) {
    const ref = createRef();
    // const changeEvent = onchange && (event => onchange(ref.current, event.srcElement.value));

    useEffect(() => {
        const instance = ref.current;
        
        if(!this.quill){
            this.quill = new Quill(instance, {
                theme: 'snow'
            });

            onchange && this.quill.on('text-change', (delta, oldDelta, source) => onchange(instance, this.quill.root.innerHTML));
        }

        if(this.quill.root.innerHTML != initialValue || null){
            this.quill.root.innerHTML = initialValue || null;
        }
    }, [initialValue]);

    return html`
        <div ref=${ref} class="cloudy-ui-form-input">
        <//>
    `;
}

export default Html;