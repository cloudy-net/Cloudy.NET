import FieldControl from '../field-control.js';

class HtmlControl extends FieldControl {
    constructor(fieldModel, value, app, blade) {
        var container = document.createElement('div');
        super(container);

        var input = document.createElement('div');
        input.classList.add('cloudy-ui-form-input');
        input.innerHTML = value || null;
        container.append(input);

        this.triggerChange(input.innerHTML)

        this.quill = new Quill(input, {
            theme: 'snow'
        });

        this.quill.on('text-change', (delta, oldDelta, source) => this.triggerChange(this.quill.root.innerHTML));

        this.onSet(value => input.innerHTML = value || null);
    }
}

export default HtmlControl;