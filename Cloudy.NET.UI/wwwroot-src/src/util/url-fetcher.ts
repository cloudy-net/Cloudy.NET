﻿import notificationManager from "../notification/notification-manager";

class UrlFetcher {
    async fetch(url: string, parameters: any, errorMessage: string, errorCodes?: { [key: number]: (response: Response) => any }) {
        let shouldDisplay = true;

        try {
            var response = await fetch(url, parameters);

            if (errorCodes && errorCodes[response.status]) {
                shouldDisplay = false;
                throw errorCodes[response.status](response);
            }

            if (!response.ok) {
                var text = await response.text();

                if (text) {
                    var error = text.split('\n')[0];
                    var location = text.match(/at ([^(]+)\([^)]*\) in (..[^:]+):line (\d+)/);

                    if (location) {
                        var methodNameSplit = location[1].split('.');
                        var methodName = methodNameSplit[methodNameSplit.length - 2] + '.' + methodNameSplit[methodNameSplit.length - 1];
                        var filenameSplit = location[2].split(/[/\\]/);
                        var filename = filenameSplit[filenameSplit.length - 1];
                        var lineNumber = location[3];

                        error += ` --- at ${methodName}() in ${filename}:${lineNumber}`;
                    }

                    throw new Error(error);
                } else {
                    text = response.statusText;
                }

                throw new Error(`${response.status} (${text})`);
            }

            return await response.json();
        } catch (error: any) {
            if (shouldDisplay) {
                notificationManager.addNotification((item: any) => item.setText(`${errorMessage} --- ${error.message}`));
            }
            throw error;
        }
    }
}

export default new UrlFetcher();