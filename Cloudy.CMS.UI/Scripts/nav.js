
class Nav {
    constructor(portal) {
        this.portal = portal;
        this.toggle = document.createElement('poetry-ui-portal-nav-toggle');
        portal.element.appendChild(this.toggle);

        this.toggle.tabIndex = 0;
        var hideToggle = () => {
            if (!this.toggle.classList.contains('poetry-ui-active')) {
                return;
            }

            this.toggle.classList.remove('poetry-ui-active');
            this.element.classList.add('poetry-ui-hidden');
        };
        this.toggle.addEventListener('click', () => {
            if (!this.toggle.classList.contains('poetry-ui-active')) {
                this.toggle.classList.add('poetry-ui-active');
                this.element.classList.remove('poetry-ui-hidden');
            } else {
                hideToggle();
            }
        });
        this.toggle.addEventListener("keyup", event => {
            if (event.keyCode != 13) {
                return;
            }

            event.preventDefault();
            this.toggle.click();
        });
        document.documentElement.addEventListener('click', event => {
            if (!event.target.matches('poetry-ui-portal-nav, poetry-ui-portal-nav-toggle')) {
                hideToggle();
            }
        });

        this.element = document.createElement('poetry-ui-portal-nav');
        this.element.classList.add('poetry-ui-hidden');
        portal.element.append(this.element);

        this.appDescriptorsPromise = fetch('App/GetAll', { credentials: 'include' }).then(response => response.json());
        this.appDescriptorsPromise.then(appDescriptors => appDescriptors.forEach(appDescriptor => {
            var item = document.createElement('poetry-ui-portal-nav-item');

            item.tabIndex = 0;
            item.innerText = appDescriptor.Name;
            item.setAttribute('poetry-ui-app-id', appDescriptor.Id);
            item.addEventListener('click', () => portal.openApp(appDescriptor));
            item.addEventListener("keyup", event => {
                if (event.keyCode != 13) {
                    return;
                }

                event.preventDefault();
                item.click();
            });

            this.element.appendChild(item);
        }));

        if (document.readyState != 'loading') {
            this.openStartApp();
        } else {
            document.addEventListener('DOMContentLoaded', this.openStartApp);
        }
    }

    openStartApp() {
        if (!location.hash) {
            return;
        }

        var match = location.hash.substr(1).match(/^[a-z0-9-_.]+/i);

        if (match) {
            var appId = match[0];

            this.appDescriptorsPromise.then(appDescriptors => {
                var appDescriptor = appDescriptors.find(a => a.Id == appId);

                if (!appDescriptor) {
                    throw `App not found: ${appId}`;
                }

                this.portal.openApp(appDescriptor);
            });
        }
    }

    openApp(appDescriptor) {
        this.appDescriptorsPromise.then(() => {
            [...this.element.querySelectorAll('poetry-ui-portal-nav-item')].forEach(c => c.classList.remove('poetry-ui-active'));
            this.element.querySelector(`[poetry-ui-app-id="${appDescriptor.Id}"]`).classList.add('poetry-ui-active');
        });
    }
}

export default Nav;