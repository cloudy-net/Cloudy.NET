import notificationManager from "../../NotificationSupport/notification-manager.js";



class SelectItemProvider {
    async get(provider, type, value) {
        try {
            var response = await fetch(`SelectControl/GetItem?provider=${provider}&type=${type}&value=${value}`, {
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json'
                }
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

            return await response.json();
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not get item ${value} of type ${type} for select control ${provider} (${error.message})`));
        }
    }

    async getAll(provider, type) {
        try {
            var response = await fetch(`SelectControl/GetItems?provider=${provider}&type=${type}`, {
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json'
                }
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

            return await response.json();
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not get items of type ${type} for select control ${provider} (${error.message})`));
        }
    }

    async getProvider(id) {
        try {
            var response = await fetch(`SelectControl/GetProvider?id=${id}`, {
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json'
                }
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

            return await response.json();
        } catch (error) {
            notificationManager.addNotification(item => item.setText(`Could not get item ${value} for select control ${provider} (${error.message})`));
        }
    }
}

export default new SelectItemProvider();