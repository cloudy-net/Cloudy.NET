import Button from './button.js';
import FormBuilder from './FormSupport/form-builder.js';
import FieldModel from './FormSupport/field-model.js';
import TextControl from './FormSupport/Controls/text-control.js';
import notificationManager from './NotificationSupport/notification-manager.js';




/* LOGIN */

class Login {
    constructor() {
        this.container = document.createElement('poetry-ui-login-container');

        this.formTarget = document.createElement('iframe');
        this.formTarget.style.display = 'none';
        this.formTarget.name = "cloudy-login-target";
        this.formTarget.src = "about:blank";
        this.container.append(this.formTarget);

        this.form = document.createElement('form');
        this.form.classList.add('poetry-ui-login');
        this.form.target = "cloudy-login-target";
        this.form.action = "about:blank";
        this.form.addEventListener('submit', event => {
            this.form.style.opacity = 0.5;
            fetch('Login', {
                credentials: 'include',
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(target)
            })
                .then(response => response.json())
                .then(result => {
                    this.form.style.opacity = '';

                    if (!result.success) {
                        notificationManager.addNotification(n => n.setText(result.message));
                        return;
                    }

                    location.href = new URLSearchParams(window.location.search).get('ReturnUrl');
                })
                .catch(e => {
                    this.form.style.opacity = '';
                    notificationManager.addNotification(n => n.setText(e.toString()));
                });
        });
        this.container.append(this.form);

        this.header = document.createElement('poetry-ui-login-header');
        this.form.append(this.header);

        this.content = document.createElement('poetry-ui-login-content');
        this.form.append(this.content);

        var target = {};

        new FormBuilder([
            new FieldModel({
                id: 'Email',
                label: 'Email',
                camelCaseId: 'email',
                control: 'email',
            }, TextControl, null),
            new FieldModel({
                id: 'Password',
                label: 'Password',
                camelCaseId: 'password',
                control: 'password',
            }, TextControl, null),
        ]).build(target).then(form => {
            this.content.append(form.element);
            form.element.querySelector('input[name="password"]').type = 'password';
            form.element.querySelector('input[name="email"]').focus();
        });

        this.footer = document.createElement('poetry-ui-login-footer');
        this.form.append(this.footer);

        var button = document.createElement('button');
        button.classList.add('poetry-ui-button');
        button.classList.add('primary');
        button.innerText = 'Login';
        button.type = 'submit';
        this.footer.append(button);

        if (document.readyState != 'loading') {
            document.body.appendChild(this.container);
        } else {
            document.addEventListener('DOMContentLoaded', document.body.appendChild(this.container));
        }
    }

    setTitle(value) {
        this.header.innerText = value;
    }

    openApp(appDescriptor) {
        [...this.element.querySelectorAll('poetry-ui-app')].forEach(a => this.element.removeChild(a));

        this.nav.openApp(appDescriptor);

        history.pushState(null, null, `#${appDescriptor.id}`);

        var open = app => {
            this.element.appendChild(app.element);
            app.openStartBlade();
        }

        if (this.apps[appDescriptor.id]) {
            open(this.apps[appDescriptor.id]);
            return;
        }

        import(`./${appDescriptor.modulePath}`).then(module => open(new module.default()));
    }
}

export default Login;