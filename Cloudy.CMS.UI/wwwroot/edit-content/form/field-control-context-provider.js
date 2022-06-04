import { useEffect, useState } from '../../lib/preact.hooks.module.js';
import notificationManager from '../../NotificationSupport/notification-manager.js';
import html from '../../util/html.js';
import urlFetcher from '../../util/url-fetcher.js';
import FieldControlContext from './field-control-context.js';

function FieldControlContextProvider({ children }) {
    const [state, setState] = useState(null);

    useEffect(() => {
        urlFetcher
        .fetch('Control/ModulePaths', { credentials: 'include' }, 'Could not get module paths for form field controls')
        .then(response => 
            Promise.all(
                Object.entries(response)
                .map(async ([controlType, modulePath]) => {
                    if (modulePath.indexOf('/') == 0 || modulePath.indexOf('://') != -1) { // absolute urls
                        if (location.href.indexOf('https://') == 0 && modulePath.indexOf(`http://${location.hostname}`) == 0) { // revert SSL termination on own hostname
                            modulePath = modulePath.replace('http://', 'https://');
                        }
                    } else { // relative urls
                        modulePath = `../../${modulePath}`;
                    }

                    try {
                        return [controlType, (await import(modulePath)).default];
                    } catch(error) {
                        notificationManager.addNotification(item => item.setText(`Could not load field control \`${controlType}\` --- ${error.message} (${error.name})`));
                        return [controlType, null];
                    }
                })
            )
            .then(entries => Object.fromEntries(entries))
        )
        .then(result => setState(result));
    }, []);

    return html`
        <${FieldControlContext.Provider} value=${state}>
            ${state && children}
        <//>
    `;
}

export default FieldControlContextProvider;
