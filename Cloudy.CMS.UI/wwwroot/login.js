import Button from './button.js';
import FormBuilder from './FormSupport/form-builder.js';
import FieldModel from './FormSupport/field-model.js';
import TextControl from './FormSupport/Controls/text-control.js';
import notificationManager from './NotificationSupport/notification-manager.js';




/* LOGIN */

class Login {
    constructor() {
        this.container = document.createElement('cloudy-ui-login-container');

        this.formTarget = document.createElement('iframe');
        this.formTarget.style.display = 'none';
        this.formTarget.name = "cloudy-login-target";
        this.formTarget.src = "about:blank";
        this.container.append(this.formTarget);

        this.form = document.createElement('form');
        this.form.classList.add('cloudy-ui-login');
        this.form.target = "cloudy-login-target";
        this.form.action = "about:blank";
        this.form.addEventListener('submit', event => this.authorize());
        this.container.append(this.form);

        this.header = document.createElement('cloudy-ui-login-header');
        this.form.append(this.header);

        this.content = document.createElement('cloudy-ui-login-content');
        this.form.append(this.content);

        this.target = {};

        var fieldModels = [
            new FieldModel({
                id: 'Email',
                label: 'Email',
                camelCaseId: 'email',
                control: { id: 'text', parameters: {} },
            }, TextControl, null),
            new FieldModel({
                id: 'Password',
                label: 'Password',
                camelCaseId: 'password',
                control: { id: 'password', parameters: {} },
            }, TextControl, null),
        ];
        new FormBuilder(null, null).build(this.target, fieldModels).then(form => {
            this.content.append(form.element);
            form.element.querySelector('input[name="email"]').focus();
        });

        this.footer = document.createElement('cloudy-ui-login-footer');
        this.form.append(this.footer);

        var button = document.createElement('button');
        button.classList.add('cloudy-ui-button');
        button.classList.add('primary');
        button.innerText = 'Login';
        button.type = 'submit';
        this.footer.append(button);
    }

    async authorize() {
        this.form.style.opacity = 0.5;

        try {
            var response = await fetch('Login/Authorize', {
                credentials: 'include',
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(this.target)
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

            if (!result.success) {
                setTimeout(() => this.form.style.opacity = '', 500);
                notificationManager.addNotification(n => n.setText(result.message));
                return;
            }

            setTimeout(() => this.form.style.opacity = '', 2500);
            location.href = new URLSearchParams(window.location.search).get('ReturnUrl');
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not save content (${error.message})`));
            setTimeout(() => this.form.style.opacity = '', 500);
            throw error;
        }
    }

    setTitle(value) {
        this.header.innerText = value;
    }

    appendTo(element) {
        element.append(this.container);
    }
}

export default Login;