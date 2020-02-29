import App from '/Admin/files/app.js';
import Blade from '/Admin/files/blade.js';
import LinkButton from '/Admin/files/link-button.js';

class ListContentTypesBlade extends Blade{
    constructor(app) {
        super();
        this.setTitle('Welcome screen');
        var image = `<img class="cloudy-ui-help-illustration" src="/demo/feeling-proud.svg" alt="Illustration of a desktop with a computer, signifying work that can be commenced.">`;
        var heading = '<h2 class="cloudy-ui-help-heading">You\'ve started the demo project</h2>';
        var text = '<p>What you would like to do now?</p>';

        var helpContainer = document.createElement('cloudy-ui-help-container');
        helpContainer.innerHTML = image + heading + text;

        var button = new LinkButton('Admin UI', '/Admin').setPrimary();
        helpContainer.append(button.element);

        var button = new LinkButton('Website', 'https://cloudy-cms.net/').setPrimary();
        button.element.style.marginLeft = '8px';
        helpContainer.append(button.element);

        this.setContent(helpContainer);
    }
}

var app = new App();
app.element.style.top = '0';
document.body.append(app.element);
app.open(new ListContentTypesBlade(app));

export default app;