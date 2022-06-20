import { useEffect, useRef, useState } from '../../lib/preact.hooks.module.js';
import html from '../../util/html.js';
import Button from '../button/button.js';
import List from '../list/list.js';

function ContextMenu({ position, children }) {
    return html`
        <cloudy-ui-context-menu class=${position && `cloudy-ui-context-menu-${position}`}>
            <${List}>
                ${children}
            <//>
        <//>
    `;
}

function PopupMenu({ buttonClass, text, children, position }) {
    const [visible, setVisible] = useState(false);
    const ref = useRef(null);

    useEffect(() => {
        const instance = ref.current;
        
        const callback = ({ target }) => {
            setTimeout(() => {
                let parent = target;
    
                while(parent){
                    if(parent == instance){
                        return;
                    }
    
                    parent = parent.parentNode;
                }
    
                setVisible(false);
            }, 10);
        };

        document.addEventListener("click", callback);

        return () => {
            document.removeEventListener("click", callback);
        };
    }, []);

    return html`
        <cloudy-ui-context-menu-outer ref=${ref}>
            <${Button} cssClass="${buttonClass} ${visible ? 'cloudy-ui-active' : null}" text=${text} onClick=${() => setVisible(!visible)}/>
            ${visible && html`<${ContextMenu} position=${position} children=${children}/>`}
        <//>
    `;
}

export default PopupMenu;