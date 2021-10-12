import FieldControl from '../field-control.js';
import urlFetcher from '../../url-fetcher.js';

class DropdownControl extends FieldControl {
    constructor(fieldModel, value, app, blade) {
        var select = document.createElement('select');
        select.classList.add('cloudy-ui-form-input');
        select.value = value || null;

        super(select);

        select.addEventListener('change', () => this.triggerChange(select.value || null));

        this.onSet(v => value = select.value = v || null);

        this.load(fieldModel.descriptor.control.parameters['provider']);
    }

    async load(provider) {
        var items = await urlFetcher.fetch(`DropdownControl/GetOptions?provider=${provider}`, {
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        }, 'Could not get options for dropdown control');

        items.forEach(item => {
            var option = document.createElement('option');

            option.value = item.value;
            option.innerText = item.text;

            this.element.append(option);
        });

        this.element.value = value;
    }
}

export default DropdownControl;