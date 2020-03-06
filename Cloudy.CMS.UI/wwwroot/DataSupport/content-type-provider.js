import notificationManager from '../NotificationSupport/notification-manager.js';



/* CONTENT TYPE PROVIDER */

class ContentTypeProvider {
    all;

    async getAll() {
        try {
            if (!this.all) {
                this.all = fetch('ContentTypeProvider/GetAll', { credentials: 'include' })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error(`${response.status} (${response.statusText})`);
                        }

                        return response.json();
                    });
            }

            return await this.all;
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not get content types (${error.name}: ${error.message})`));
            throw error;
        }
    }

    async get(id) {
        var contentTypes = await this.getAll();

        return contentTypes.find(contentType => contentType.id == id);
    }
}

export default new ContentTypeProvider();