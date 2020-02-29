


/* CLOSE BUTTON */

class BladeCloseButton {
    constructor() {
        this.element = document.createElement('cloudy-ui-blade-title-close');
        this.element.setAttribute('tabindex', 0);
        this.element.addEventListener('click', () => this.element.dispatchEvent(new CustomEvent('cloudy-ui-close-blade', { bubbles: true, detail: { parameters: [] } })));
        this.element.addEventListener("keyup", event => {
            if (event.keyCode != 13) {
                return;
            }

            event.preventDefault();
            this.element.click();
        });
    }
}

export default BladeCloseButton;