
import html from '../../util/html.js';
import { useContext } from '../../lib/preact.hooks.module.js';
import fieldControlContext from './field-control-context.js';

export default function SimpleField({ fieldDescriptor, state, path }) {
    const fieldControl = useContext(fieldControlContext)[fieldDescriptor.control && fieldDescriptor.control.id];

    if(!fieldControl){
        return html`
            <cloudy-ui-info-message>Error: Control ${fieldDescriptor.control && fieldDescriptor.control.id} not found<//>
        `;
    }

    return html`
        <${fieldControl} path=${path} fieldDescriptor=${fieldDescriptor} state=${state}/>
    `;
}