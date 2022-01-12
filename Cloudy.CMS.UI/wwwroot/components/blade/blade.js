import html from '../../util/html.js';
import { useCallback } from '../../lib/preact.hooks.module.js';

function Blade(props) {
    const onClose =  useCallback(() => {
        props.onclose && props.onclose();
    }, []);

    return html`
        <cloudy-ui-blade>
            <cloudy-ui-blade-title>
                <cloudy-ui-blade-title-text>${props.title}<//>
                <cloudy-ui-blade-toolbar>${props.toolbar}<//>
                <cloudy-ui-blade-close onclick=${() => onClose()}><//>
            <//>
           
                    ${props.children}
                  
        <//>
    `;
}

export default Blade;