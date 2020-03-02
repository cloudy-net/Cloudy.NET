import notificationManager from "../NotificationSupport/notification-manager.js";
import Button from "../button.js";



/* HELP SECTION LOADER */

class HelpSectionLoader {
    static load(id, strings, actions) {
        strings = Object.entries(strings);
        var translate = text => strings.reduce((string, [key, value]) => string.replace(`{${key}}`, value), text);

        var settings = fetch(`Content/GetSettings`, { credentials: 'include' })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`${response.status} (${response.statusText})`);
                }

                return response.json();
            })
            .catch(error => notificationManager.addNotification(item => item.setText(`Could not get settings for content app (${error.name}: ${error.message})`)));

        return settings.then(settings =>
            fetch(`${settings.helpSectionBaseUri}/${id}.json`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error(`${response.status} (${response.statusText})`);
                    }

                    return response.json();
                })
                .catch(error => notificationManager.addNotification(item => item.setText(`Could not get help section ${id} (${error.name}: ${error.message})`)))
                .then(content => {
                    var element = document.createElement('cloudy-ui-help-container');

                    element.insertAdjacentHTML('beforeend', `<img class="cloudy-ui-help-illustration" src="${content.image.src}" alt="${content.image.alt}">`);

                    element.insertAdjacentHTML('beforeend', `<h2 class="cloudy-ui-help-heading">${translate(content.heading)}</h2>`);
                    element.insertAdjacentHTML('beforeend', translate(content.text));

                    if (content.actions.length) {
                        var buttons = document.createElement('div');
                        buttons.style.textAlign = 'center';
                        element.append(buttons);

                        var actionsContent = Object.fromEntries(content.actions.map(a => [a.id, a.text]));
                        Object.entries(actions).forEach(([key, callback]) => new Button(translate(actionsContent[key])).setPrimary().onClick(callback).appendTo(buttons));
                    }

                    if (content.code) {
                        var code = '<pre class="cloudy-ui-help-code">public class MyClass : …, INameable\n' +
                            '{\n' +
                            '    …\n' +
                            '    public string Name { get; set; }\n' +
                            '}</pre>';

                        element.insertAdjacentHTML('beforeend', code);
                    }

                    return element;
                }));
    }
}

export default HelpSectionLoader;