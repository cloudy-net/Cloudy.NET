import { useEffect, useRef } from '../../../lib/preact.hooks.module.js';
import getIntermediateSimpleValue from '../../../util/get-intermediate-simple-value.js';
import html from '../../../util/html.js';
import stateManager from '../../state-manager.js';

function Html({ fieldDescriptor, state, readonly, path }) {
    const ref = useRef(null);

    useEffect(() => {
        const instance = ref.current;
        
        if(!this.quill){
            this.quill = new Quill(instance, {
                theme: 'snow'
            });
        }

        const callback = () => stateManager.registerSimpleChange(state.contentReference, path, this.quill.root.innerHTML.replace(/^\s*<p\s*>\s*<br\s*\/?>\s*<\/p\s*>\s*$/ig, ''));

        this.quill.root.innerHTML = getIntermediateSimpleValue(state.referenceValues, path, state.simpleChanges) || null;

        this.quill.on('text-change', callback);

        return () => {
            this.quill.off('text-change', callback);
        }
    }, [state]);

    return html`
        <div ref=${ref} class="cloudy-ui-form-input">
        <//>
    `;
}

export default Html;