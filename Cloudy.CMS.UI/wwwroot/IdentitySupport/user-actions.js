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
        this.app = app;
    }

    async open() {
        var name = this.user.username || `User ${this.user.id}`;

        this.setTitle(`Change password for ${name}`);

        var target = {
            userId: this.user.id,
        };

        this.setContent(await new FormBuilder(this.app, this).build(target, await fieldModelBuilder.getFieldModels('Cloudy.CMS.Identity.ChangePassword')));

        var save = async () => {
            try {
                var response = await fetch('Identity/ChangePassword', {
                        credentials: 'include',
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(target)
                    });

                if (!response.ok) {
                    var text = await response.text();

                    if (text) {
                        throw new Error(text.split('\n')[0]);
                    } else {
                        text = response.statusText;
                    }

                    throw new Error(`${response.status} (${text})`);
                }

                var result = await response.json();

                if (result.success) {
                    notificationManager.addNotification(item => item.setText('Password changed.'));
                    this.app.removeBlade(this);
                } else {
                    var errors = document.createElement('ul');
                    result.errors.forEach(error => {
                        var item = document.createElement('li');
                        item.innerText = error.description;
                        errors.append(item);
                    });
                    notificationManager.addNotification(item => item.setText('Error changing password:', errors));
                }
            } catch (error) {
                notificationManager.addNotification(item => item.setText(`Could not change password --- ${error.message}`));
                throw error;
            }
        };

        this.setFooter(new Button('Save').setPrimary().onClick(() => save()), new Button('Cancel').onClick(() => this.app.removeBlade(this)));
    }
}