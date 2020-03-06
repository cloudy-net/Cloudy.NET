import FieldControl from '../field-control.js';
import notificationManager from '../../NotificationSupport/notification-manager.js';

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
        try {
            var response = await fetch(`DropdownControl/GetOptions?provider=${provider}`, {
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                var text = await response.text;

                if (text) {
                    throw new Error(text.split('\n')[0]);
                } else {
                    text = response.statusText;
                }

                throw new Error(`${response.status} (${text})`);
            }

            var items = await response.json();

            items.forEach(item => {
                var option = document.createElement('option');

                option.value = item.value;
                option.innerText = item.text;

                this.element.append(option);
            });

            this.element.value = value;
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not get options for dropdown control ${provider} (${error.message})`));
        }
    }
}

export default DropdownControl;