import "./content-type-provider.js";
import notificationManager from "../NotificationSupport/notification-manager.js";
import urlFetcher from "../url-fetcher.js";

/* CONTENT SAVER */

class ContentSaver {
    async save(changes) {
        var result = await urlFetcher.fetch("SaveContent/SaveContent", {
            credentials: "include",
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                changes
            }),
        }, 'Could not save content');

        if (!result.success) {
            var errors = document.createElement("ul");
            Object.entries(result.validationErrors).forEach((error) => {
                var item = document.createElement("li");
                item.innerText = `${error[0]}: ${error[1]}`;
                errors.append(item);
            });
            notificationManager.addNotification((item) =>
                item.setText(`Error saving:`, errors)
            );
            return;
        }

        notificationManager.addNotification((item) => item.setText('Content has been saved.'));

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