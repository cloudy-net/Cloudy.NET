import App from '/Admin/files/app.js';
import Blade from '/Admin/files/blade.js';
import LinkButton from '/Admin/files/link-button.js';

class ListContentTypesBlade extends Blade{
    constructor(app) {
        super();
        this.setTitle('Welcome screen');
        var image = `<img class="poetry-ui-help-illustration" src="/demo/feeling-proud.svg" alt="Illustration of a desktop with a computer, signifying work that can be commenced.">`;
        var text = '<p>Choose what you would like to do now:</p>';

        var helpContainer = document.createElement('poetry-ui-help-container');
        helpContainer.innerHTML = image + text;

        var button = new LinkButton('To the Admin UI', '/Admin').setPrimary();
        helpContainer.append(button.element);

        this.setContent(helpContainer);
    }
}

var app = new App();
app.element.style.top = '0';
document.body.append(app.element);
app.open(new ListContentTypesBlade(app));

export default app;