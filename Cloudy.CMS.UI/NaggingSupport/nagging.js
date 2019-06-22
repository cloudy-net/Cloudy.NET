import Button from '../Scripts/button.js';
import NotificationManager from '../NotificationSupport/Scripts/notification-manager.js';

fetch(`License/Status`, {
    credentials: 'include',
    method: 'Get',
})
    .then(response => response.json())
    .then(data => {
        if (data.License != null) {
            return;
        }
        if (data.DontNagOnLocalhost && location.hostname == 'localhost') {
            return;
        }

        function onClose() {
            if (location.hostname != 'localhost') {
                return;
            }

            if (cookieExists()) {
                return;
            }

            var ping = new Button('OK, ping');

            var notification;

            NotificationManager.addNotification(n => notification = n.setContent(`Phone home?`).setText('Is it OK to ping poetry-cms.net so we know a new installation was done?').setButtons(ping).setSource('Nagging center'));

            ping.onClick(
                () =>
                    fetch('https://cloudy-cms.net/ping-install', {
                        method: 'Post',
                    })
                        .then(() => {
                            saveCookie();
                            notification.close();
                        })
            );
        }

        var order = document.createElement('a');
        order.classList.add('poetry-ui-button');
        order.href = 'https://cloudy-cms.net/#order';
        order.setAttribute('target', '_blank');
        order.innerText = 'Order license';
        NotificationManager.addNotification(
            notification =>
                notification
                    .setContent(`License missing!`)
                    .setText(
                        location.hostname == 'localhost' ?
                            'Request a license now, or use .AddCMSUI(ui => ui.DontNagOnLocalhost()) to wait.' :
                            ''
                    )
                    .setButtons(order)
                    .setSource('Nagging center')
                    .onClose(() => onClose())
        );
    });



/* COOKIE FUNCTIONS */

var cookieName = `${location.hostname}${location.port}nag`;

function cookieExists() {
    return ("; " + document.cookie).indexOf(`; ${cookieName}=true`) != -1;
}

function saveCookie() {
    document.cookie = `${cookieName}=true; expires='expires=Fri, 31 Dec 9999 23:59:59 GMT;'; path=/`
}

function clearCookie() {
    document.cookie = `${cookieName}=false; expires='expires=Fri, 31 Dec 2000 23:59:59 GMT;'; path=/`
}