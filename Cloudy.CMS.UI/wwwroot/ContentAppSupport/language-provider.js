import notificationManager from '../NotificationSupport/notification-manager.js';



/* LANGUAGE PROVIDER */

class LanguageProvider {
    all;

    async getAll() {
        try {
            if (!this.all) {
                this.all = (async () => {
                    var response = await fetch('LanguageProvider/GetAll', { credentials: 'include' });
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
            notificationManager.addNotification(item => item.setText(`Could not get languages --- ${error.message}`));
            throw error;
        }
    }

    async get(id) {
        var languages = await this.getAll();

        return languages.find(language => language.id == id);
    }
}

export default new LanguageProvider();