import notificationManager from '../NotificationSupport/notification-manager.js';



/* CONTENT GETTER */

class ContentGetter {
    contentByContentTypeAndId = {};

    /**
     * Gets content of the specified type.
     * @param {string} contentId
     * @param {string} contentTypeId
     */
    async get(contentId, contentTypeId) {
        if (!this.contentByContentTypeAndId[contentTypeId]) {
            this.contentByContentTypeAndId[contentTypeId] = {};
        }

        if (this.contentByContentTypeAndId[contentTypeId][contentId]) {
            return this.contentByContentTypeAndId[contentTypeId][contentId];
        }

        try {
            const url = `ContentGetter/Get?contentId=${contentId}&contentTypeId=${contentTypeId}`;
            const response = await fetch(url, { credentials: 'include' });

            if (!response.ok) {
                var text = await response.text();

                if (text) {
                    throw new Error(text.split('\n')[0]);
                } else {
                    text = response.statusText;
                }

                throw new Error(`${response.status} (${text})`);
            }

            var content = await response.json();
            this.contentByContentTypeAndId[contentTypeId][contentId] = content;
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not get content ${contentId} (${contentTypeId}) --- ${error.message}`));
            throw error;
        }

        return content;
    }

    /**
     * Updates the cache of a specified content.
     * @param {object} content
     */
    set(content) {
        if (!this.contentByContentTypeAndId[content.contentTypeId]) {
            this.contentByContentTypeAndId[content.contentTypeId] = {};
        }

        this.contentByContentTypeAndId[contentTypeId][contentId] = content.content;
    }
}

export default new ContentGetter();