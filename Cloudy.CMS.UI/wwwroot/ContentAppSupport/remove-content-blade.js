import Blade from '../blade.js';
import Button from '../button.js';
import notificationManager from '../NotificationSupport/notification-manager.js';
import urlFetcher from '../url-fetcher.js';
import nameGetter from './utils/name-getter.js';
import contentGetter from './utils/content-getter.js';
import primaryKeyProvider from './utils/primary-key-provider.js';

class RemoveContentBlade extends Blade {
    onCompleteCallbacks = [];

    constructor(app, contentType, contentId) {
        super();
        this.contentType = contentType;
        this.contentId = contentId;
    }

    async open() {
        const content = await contentGetter.get(this.contentId, this.contentType.id);
        const name = nameGetter.getNameOf(content, this.contentType);

        this.setTitle(`Remove ${name}`);

        var container = document.createElement('div');
        container.style.padding = '16px';
        container.innerText = 'Are you sure?';

        this.setContent(container);

        var saveButton = new Button('Remove')
            .setPrimary()
            .onClick(async () => {
                await urlFetcher.fetch('RemoveContent/RemoveContent', {
                    credentials: 'include',
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        keyValues: primaryKeyProvider.getFor(content, this.contentType),
                        contentTypeId: this.contentType.id
                    })
                }, 'Could not remove content');

                var name = null;
                if (!this.contentType.isSingleton) {
                    if (this.contentType.isNameable) {
                        name = this.contentType.nameablePropertyName ? content[this.contentType.nameablePropertyName] : content.name;
                    }
                    if (!name) {
                        name = content.id;
                    }
                }

                notificationManager.addNotification(item => item.setText(`Removed ${this.contentType.name} ${name || ''}`));
                this.onCompleteCallbacks.forEach(callback => callback(content));
                app.removeBlade(this);
            });
        var cancelButton = new Button('Cancel').onClick(() => app.removeBlade(this));

        this.setFooter(saveButton, cancelButton);
    }

    onComplete(callback) {
        this.onCompleteCallbacks.push(callback);

        return this;
    }
}

export default RemoveContentBlade;