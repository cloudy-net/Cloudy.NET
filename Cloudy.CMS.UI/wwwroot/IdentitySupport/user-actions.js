import Blade from "../blade.js";
import FormBuilder from "../FormSupport/form-builder.js";
import Button from "../button.js";

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
                .then(body => {
                    console.log(body)
                    //app.close(this);
                })
        };

        this.setFooter(new Button('Save').setPrimary().onClick(() => save()), new Button('Cancel').onClick(() => app.close(this)));
    }
}