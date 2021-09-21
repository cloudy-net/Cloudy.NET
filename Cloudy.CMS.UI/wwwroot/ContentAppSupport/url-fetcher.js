import notificationManager from "../NotificationSupport/notification-manager.js";

class UrlFetcher {
    async fetch(url, parameters, errorMessage) {
        try {
            var response = await fetch(url, parameters);

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
            notificationManager.addNotification(item => item.setText(`${errorMessage} --- ${error.message}`));
            throw error;
        }
    }
}

export default new UrlFetcher();