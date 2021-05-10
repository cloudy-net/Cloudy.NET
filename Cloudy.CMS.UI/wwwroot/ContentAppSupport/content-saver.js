import './content-type-provider.js';
import notificationManager from '../NotificationSupport/notification-manager.js'
import contentTypeProvider from '../ContentAppSupport/content-type-provider.js'



/* CONTENT SAVER */

class ContentSaver {
    async save(change) {
        try {
            var contentType = await contentTypeProvider.get(change.contentTypeId);

            var response = await fetch('SaveContent/SaveContent', {
                credentials: 'include',
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    id: change.id,
                    contentType: change.contentType,
                    contentAsJson: change.contentAsJson,
                })
            });

            if (!response.ok) {
                var text = await response.text();

                if (text) {
                    throw new Error(text.split('\n')[0]);
                } else {
                    text = response.statusText;
                }

                throw new Error(`${response.status} (${text})`);
            }

            var result = await response.json();
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not save content --- ${error.message}`));
            throw error;
        }

        if (!result.success) {
            var errors = document.createElement('ul');
            Object.entries(result.validationErrors).forEach(error => {
                var item = document.createElement('li');
                item.innerText = `${error[0]}: ${error[1]}`;
                errors.append(item);
            });
            notificationManager.addNotification(item => item.setText(`Error saving ${contentType.name}:`, errors));
            return;
        }

        // var name = null;

        // if (!contentType.isSingleton) {
        //     if (contentType.isNameable) {
        //         name = contentType.nameablePropertyName ? content[contentType.nameablePropertyName] : content.name;
        //     }
        //     if (!name) {
        //         name = content.id;
        //     }
        // }

        // if (!content.id) {
        //     notificationManager.addNotification(item => item.setText(`Created ${contentType.name} ${name || ''}`));
        //     app.removeBlade(this);
        // } else {
        //     notificationManager.addNotification(item => item.setText(`Updated ${contentType.name} ${name || ''}`));
        // }

        //this.onCompleteCallbacks.forEach(callback => callback(this.content));
    }
}

export default new ContentSaver();