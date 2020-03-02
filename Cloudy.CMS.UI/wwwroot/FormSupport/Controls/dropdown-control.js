import FieldControl from '../field-control.js';

class DropdownControl extends FieldControl {
    constructor(fieldModel, value, app) {
        var select = document.createElement('select');
        select.classList.add('cloudy-ui-form-input');
        select.value = value || null;

        super(select);

        select.addEventListener('change', () => this.triggerChange(select.value || null));

        this.onSet(v => value = select.value = v || null);

        fetch(`DropdownControl/GetOptions?provider=${fieldModel.descriptor.control.parameters['provider']}`, {
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`${response.status} (${response.statusText})`);
                }

                return response.json();
            })
            .catch(error => notificationManager.addNotification(item => item.setText(`Could not get options for dropdown control ${fieldModel.descriptor.control.parameters['provider']} (${error.name}: ${error.message})`)))
            .then(items => {
                items.forEach(item => {
                    var option = document.createElement('option');

                    option.value = item.value;
                    option.innerText = item.text;

                    select.append(option);
                });

                select.value = value;
            });
    }
}

export default DropdownControl;