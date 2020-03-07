import notificationManager from "../NotificationSupport/notification-manager.js";



class ContentGetter {
    async get(id, contentTypeId) {
        try {
            var response = await fetch(`Data/ContentGetter/Get?id=${id}&contentTypeId=${contentTypeId}`, { credentials: 'include' });

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
            notificationManager.addNotification(item => item.setText(`Could not get content with id ${id} (${error.message})`));
        }
    }
}

export default new ContentGetter();