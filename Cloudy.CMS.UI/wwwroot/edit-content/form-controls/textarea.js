import html from '../../util/html.js';

function Textarea(props) {
    const { fieldModel, initialValue, readonly, onclick } = props;
    return html`
        <textarea
            type=text
            class=cloudy-ui-form-input
            name=${fieldModel.descriptor.id}
            onclick=${onclick}
            value=${initialValue}
            rows=${fieldModel.descriptor.control.parameters.options && fieldModel.descriptor.control.parameters.options.rows ? fieldModel.descriptor.control.parameters.options.rows.value : 8}
            readonly=${readonly}
        >
        <//>
    `;
}

export default Textarea;