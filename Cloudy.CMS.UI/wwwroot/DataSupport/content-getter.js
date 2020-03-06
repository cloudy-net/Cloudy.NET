import notificationManager from "../NotificationSupport/notification-manager.js";



class ContentGetter {
    async get(id, contentTypeId) {
        try {
            var response = await fetch(`Data/ContentGetter/Get?id=${id}&contentTypeId=${contentTypeId}`, { credentials: 'include' });

            if (!response.ok) {
                throw new Error(`${response.status} (${response.statusText})`);
            }

            return await response.json();
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not get content with id ${id} (${error.name}: ${error.message})`));
        }
    }
}

export default new ContentGetter();