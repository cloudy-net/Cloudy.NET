import Blade from "../blade.js";
import FormBuilder from "../FormSupport/form-builder.js";
import Button from "../button.js";
import notificationManager from "../NotificationSupport/notification-manager.js";

export default (menu, user, blade, app) => menu.addItem(item => item.setText('Change password').onClick(() => app.openAfter(new ChangePasswordBlade(user, app), blade)));

class ChangePasswordBlade extends Blade {
    constructor(user, app) {
        super();

        this.setTitle(`Change password for ${user.username}`);

        var target = {
            userId: user.id,
        };

        new FormBuilder("Cloudy.CMS.Identity.ChangePassword", app).build(target, {}).then(form => this.setContent(form));

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
                .catch(error => notificationManager.addNotification(item => item.setText(`Could not change password (${error.name}: ${error.message})`)));
                .then(result => {
                    if (result.success) {
                        notificationManager.addNotification(item => item.setText('Password changed.'));
                        app.close(this);
                    } else {
                        var errors = document.createElement('ul');
                        result.errors.forEach(error => {
                            var item = document.createElement('li');
                            item.innerText = error.description;
                            errors.append(item);
                        });
                        notificationManager.addNotification(item => item.setText('Error changing password:', errors));
                    }
                })
        };

        this.setFooter(new Button('Save').setPrimary().onClick(() => save()), new Button('Cancel').onClick(() => app.close(this)));
    }
}