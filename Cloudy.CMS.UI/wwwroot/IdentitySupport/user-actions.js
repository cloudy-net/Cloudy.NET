import Blade from "../blade.js";
import FormBuilder from "../FormSupport/form-builder.js";
import Button from "../button.js";
import notificationManager from "../NotificationSupport/notification-manager.js";
import fieldModelBuilder from "../FormSupport/field-model-builder.js";

export default (menu, user, blade, app) => menu.addItem(item => item.setText('Change password').onClick(() => app.addBladeAfter(new ChangePasswordBlade(user, app), blade)));

class ChangePasswordBlade extends Blade {
    constructor(user, app) {
        super();

        this.user = user;
    }

    async open() {
        this.setTitle(`Change password for ${this.user.username}`);

        var target = {
            userId: this.user.id,
        };

        this.setContent(await new FormBuilder(await fieldModelBuilder.getFieldModels('Cloudy.CMS.Identity.ChangePassword'), app, this).build(target, {}));

        var save = () => {
            fetch('Identity/ChangePassword', {
                credentials: 'include',
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(target)
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error(`${response.status} (${response.statusText})`);
                    }

                    return response.json();
                })
                .catch(error => notificationManager.addNotification(item => item.setText(`Could not change password (${error.name}: ${error.message})`)))
                .then(result => {
                    if (result.success) {
                        notificationManager.addNotification(item => item.setText('Password changed.'));
                        app.removeBlade(this);
                    } else {
                        var errors = document.createElement('ul');
                        result.errors.forEach(error => {
                            var item = document.createElement('li');
                            item.innerText = error.description;
                            errors.append(item);
                        });
                        notificationManager.addNotification(item => item.setText('Error changing password:', errors));
                    }
                });
        };

        this.setFooter(new Button('Save').setPrimary().onClick(() => save()), new Button('Cancel').onClick(() => app.removeBlade(this)));
    }
}