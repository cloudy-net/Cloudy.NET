import notificationManager from '../NotificationSupport/notification-manager.js';



/* CONTENT TYPE GROUP PROVIDER */

class ContentTypeGroupProvider {
    all;

    async getAll() {
        try {
            if (!this.all) {
                this.all = (async () => {
                    var response = await fetch('ContentTypeGroupProvider/GetAll', { credentials: 'include' });
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
            notificationManager.addNotification(item => item.setText(`Could not get content type groups (${error.message})`));
            throw error;
        }
    }
}

export default new ContentTypeGroupProvider();