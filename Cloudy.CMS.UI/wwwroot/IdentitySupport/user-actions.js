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
                .then(response => response.json())
                .then(result => {
                    if (result.success) {
                        notificationManager.addNotification(item => item.setText('Password changed.'));
                    } else {
                        var errors = document.createElement('ul');
                        result.errors.forEach(error => {
                            var item = document.createElement('li');
                            item.innerText = error.description;
                            errors.append(item);
                        });
                        notificationManager.addNotification(item => item.setText('Error changing password:', errors));
                    }
                    //app.close(this);
                })
        };

        this.setFooter(new Button('Save').setPrimary().onClick(() => save()), new Button('Cancel').onClick(() => app.close(this)));
    }
}