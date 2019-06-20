import FieldControl from '../../../Scripts/field-control.js';

class DropdownControl extends FieldControl {
    constructor(fieldModel, value, app) {
        var select = document.createElement('select');
        select.classList.add('poetry-ui-form-input');
        select.value = value || null;

        super(select);

        select.addEventListener('change', () => this.triggerChange(select.value || null));

        this.onSet(v => value = select.value = v || null);

        fetch(`Poetry.UI.FormSupport/DropdownControl/GetOptions?provider=${fieldModel.descriptor.Control.Parameters['provider']}`, {
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => response.json())
            .then(items => {
                items.forEach(item => {
                    var option = document.createElement('option');

                    option.value = item.Value;
                    option.innerText = item.Text;

                    select.append(option);
                });

                select.value = value;
            });
    }
}

export default DropdownControl;