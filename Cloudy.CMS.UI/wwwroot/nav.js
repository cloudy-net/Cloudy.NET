import List from './ListSupport/list.js';
import ListItem from './ListSupport/list-item.js';
import ContextMenu from './ContextMenuSupport/context-menu.js';
import Button from './button.js';

class Nav {
    constructor(title, apps) {

        this.element = document.createElement('cloudy-ui-portal-nav');
        document.body.append(this.element);
        this.toggle = document.createElement('cloudy-ui-portal-nav-toggle');
        this.toggle.style.display = 'none';
        this.element.appendChild(this.toggle);

        this.toggle.tabIndex = 0;
        var hideToggle = () => {
            if (!this.toggle.classList.contains('cloudy-ui-active')) {
                return;
            }

            this.toggle.classList.remove('cloudy-ui-active');
            this.menu.classList.add('cloudy-ui-hidden');
            this.menuFade.classList.add('cloudy-ui-hidden');
        };
        this.toggle.addEventListener('click', () => {
            if (!this.toggle.classList.contains('cloudy-ui-active')) {
                this.toggle.classList.add('cloudy-ui-active');
                this.menu.classList.remove('cloudy-ui-hidden');
                this.menuFade.classList.remove('cloudy-ui-hidden');
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
            if (!event.target.matches('cloudy-ui-portal-nav, cloudy-ui-portal-nav-toggle')) {
                hideToggle();
            }
        });

        this.menu = document.createElement('cloudy-ui-portal-nav-menu');
        this.menu.classList.add('cloudy-ui-hidden');
        this.element.appendChild(this.menu);

        this.menuList = new List();
        this.menuList.appendTo(this.menu);

        this.title = document.createElement('cloudy-ui-portal-nav-title');
        this.element.append(this.title);

        this.action = document.createElement('div');
        this.element.append(this.action);

        //var user = document.createElement('cloudy-ui-nav-user');
        //var userMenu = new ContextMenu();
        //userMenu.addItem(item => item.setText('Log out').onClick(() => location.href = 'Logout'));
        //userMenu.button.classList.add('cloudy-ui-nav-user-button');
        //userMenu.appendTo(user);
        //this.element.append(user);

        this.menuFade = document.createElement('cloudy-ui-portal-nav-menu-fade');
        this.menuFade.classList.add('cloudy-ui-hidden');
        this.element.appendChild(this.menuFade);

        window.addEventListener("hashchange", () => this.update());
        this.update();
    }

    setTitle(title) {
        this.title.innerText = title;
    }

    setApps(apps) {
        this.apps = apps;

        if (apps.length > 1) {
            this.toggle.style.display = '';
        }

        this.menuList.addSubHeader('Apps');
        for (var appDescriptor of apps) {
            var listItem = new ListItem();
            listItem.setText(appDescriptor.name);
            listItem.element.setAttribute('cloudy-ui-app-id', appDescriptor.id);
            listItem.onClick(() => location.hash = '#' + appDescriptor.id);
            this.menuList.addItem(listItem);
        }
    }

    setAction(...items) {
        [...this.action.childNodes].forEach(c => this.action.removeChild(c));
        items.forEach(item => this.action.append(item.element || item));
    }

    update() {
        var appId = location.hash.substr(1).split('/')[0];

        for (var item of this.menuList.element.children) {
            if (item.getAttribute('cloudy-ui-app-id') != appId) {
                item.classList.remove('cloudy-ui-active');
                continue;
            }

            item.classList.add('cloudy-ui-active');
        }
    }
}

export default new Nav();