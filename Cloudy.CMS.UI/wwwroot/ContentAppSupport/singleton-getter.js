import notificationManager from "../NotificationSupport/notification-manager.js";



class SingletonGetter {
    async get(contentTypeId) {
        try {
            var response = await fetch(`Content/GetSingleton?id=${contentTypeId}`, { credentials: 'include' });

            if (!response.ok) {
                var text = await response.text();

                if (text) {
                    throw new Error(text.split('\n')[0]);
                } else {
                    text = response.statusText;
                }

                throw new Error(`${response.status} (${text})`);
            }

            return await response.json();
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not get singleton (${error.message})`));
        }
    }
}

export default new SingletonGetter();