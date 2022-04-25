import "../data/content-type-provider.js";
import notificationManager from "../NotificationSupport/notification-manager.js";
import urlFetcher from "../util/url-fetcher.js";

class ContentSaver {
    async save(changes) {
        const response = await urlFetcher.fetch("SaveContent/SaveContent", {
            credentials: "include",
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                changes
            }),
        }, 'Could not save content');

        const success = response.results.every(r => r.success);

        if (!success) {
            response.results.filter(r => !r.success).forEach(result => {
                var errors = document.createElement("ul");
                Object.entries(result.validationErrors).forEach(error => {
                    var item = document.createElement("li");
                    item.innerText = `${error[0]}: ${error[1]}`;
                    errors.append(item);
                });
                notificationManager.addNotification((item) => item.setText(`Error saving:`, errors));
            });

            return;
        }

        notificationManager.addNotification((item) => item.setText('Content has been saved.'));
        //this.onCompleteCallbacks.forEach(callback => callback(this.content));
    }
}

export default new ContentSaver();