import notificationManager from '../NotificationSupport/notification-manager.js';



/* CONTENT TYPE PROVIDER */

class ContentTypeProvider {
    all;

    async getAll() {
        try {
            if (!this.all) {
                this.all = (async () => {
                    var response = await fetch('ContentTypeProvider/GetAll', { credentials: 'include' });
                    if (!response.ok) {
                        var text = await response.text();
                        if (text) {
                            throw new Error(text.split('\n')[0]);
                        } else {
                            text = response.statusText;
                        }

                        throw new Error(`${response.status} (${text})`);
                    }

                    return response.json();
                })();
            }

            return await this.all;
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not get content types (${error.message})`));
            throw error;
        }
    }

    async get(id) {
        var contentTypes = await this.getAll();

        return contentTypes.find(contentType => contentType.id == id);
    }
}

export default new ContentTypeProvider();