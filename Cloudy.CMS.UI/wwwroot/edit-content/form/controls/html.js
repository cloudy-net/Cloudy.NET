import { useEffect, useRef } from '../../../lib/preact.hooks.module.js';
import getIntermediateSimpleValue from '../../../util/get-intermediate-simple-value.js';
import html from '../../../util/html.js';
import stateManager from '../../state-manager.js';


document.addEventListener('click', function({ target }){ 
    if(target.classList.contains('cloudy-ui-form-field-label') && target.nextSibling && target.nextSibling.classList.contains('quill-wrapper')){
        const editor = target.nextSibling.querySelector('[contenteditable="true"]');
        if(editor) {
            editor.focus();
        }
    }
});

function Html({ fieldDescriptor, state, readonly, path }) {
    const ref = useRef(null);

    useEffect(() => {
        const instance = ref.current;
        
        if(!this.quill){
            this.quill = new Quill(instance, {
                theme: 'snow',
                modules: {
                    keyboard: {
                        bindings: {
                            tab: {
                                key: 9,
                                handler: () => true
                            }
                        }
                    }
                }
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
        <div class="cloudy-ui-form-input quill-wrapper">
            <div ref=${ref}>
            <//>
        <//>
    `;
}

export default Html;