import { useEffect } from '../../lib/preact.hooks.module.js';
import { createRef } from '../../lib/preact.module.js';
import getIntermediateValue from '../../util/get-intermediate-value.js';
import html from '../../util/html.js';

function Html({ fieldModel, initialState, onchange, readonly, path }) {
    const ref = createRef();

    useEffect(() => {
        const instance = ref.current;
        
        if(!this.quill){
            this.quill = new Quill(instance, {
                theme: 'snow'
            });
        }

        const callback = (delta, oldDelta, source) => onchange(instance, this.quill.root.innerHTML.replace(/^\s*<p\s*>\s*<br\s*\/?>\s*<\/p\s*>\s*$/ig, ''));

        this.quill.root.innerHTML = getIntermediateValue(initialState.referenceValues, path, initialState.changes) || null;

        this.quill.on('text-change', callback);

        return () => {
            this.quill.off('text-change', callback);
        }
    }, [initialState]);

    return html`
        <div ref=${ref} class="cloudy-ui-form-input">
        <//>
    `;
}

export default Html;