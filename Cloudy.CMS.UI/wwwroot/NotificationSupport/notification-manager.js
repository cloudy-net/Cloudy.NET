import Notification from './notification.js';



/* NOTIFICATION MANAGER */

class NotificationManager {
    constructor() {
        this.element = document.createElement('poetry-ui-notifications');

        var bootstrap = () => { document.body.appendChild(this.element); }

        if (document.readyState != 'loading') {
            bootstrap();
        } else {
            document.addEventListener('DOMContentLoaded', bootstrap);
        }
    }

    addNotification(callback) {
        var notification = new Notification();

        callback(notification);

        this.element.appendChild(notification.element);
    }
}

export default new NotificationManager();