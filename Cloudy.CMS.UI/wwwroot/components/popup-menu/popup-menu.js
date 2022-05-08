import { useState } from '../../lib/preact.hooks.module.js';
import html from '../../util/html.js';
import Button from '../button/button.js';
import List from '../list/list.js';

function ContextMenu(props) {
    return html`
        <cloudy-ui-context-menu>
            <${List}>
                ${props.children}
            <//>
        <//>
    `;
}

function PopupMenu({ buttonClass, text, children }) {
    const [visible, setVisible] = useState(false);

    return html`
        <cloudy-ui-context-menu-outer>
            <${Button} cssClass="${buttonClass} ${visible ? 'cloudy-ui-active' : null}" text=${text} onClick=${() => setVisible(!visible)}/>
            ${visible && html`<${ContextMenu} children=${children}/>`}
        <//>
    `;
}

export default PopupMenu;

class PopupMenu2 {
    constructor(button) {

        const callback = event => {
            if (!this.button.classList.contains('cloudy-ui-active')) {
                return;
            }

            var found = false;

            for (var position = event.target; position && position != document; position = position.parentNode) {
                if (position == this.element) {
                    found = true;
                }
            }

            if (!found) {
                this.remove();
            }
        };

        document.addEventListener('click', event => callback(event));
        document.addEventListener('keyup', event => callback(event));
    }

    toggle() {
        this.menu = document.createElement('cloudy-ui-context-menu');
        this.menu.style.opacity = 'none';
        this.list = new List();
        this.menu.append(this.list.element);
        document.body.append(this.menu);

        this.generators.forEach(generator => generator(this));

        this.menu.style.display = 'block';
        var offset = this.element.getBoundingClientRect();
        var menuOffset = this.menu.getBoundingClientRect();

        this.menu.style.top = `${offset.top - ((menuOffset.height - offset.height) / 2)}px`;

        var left = offset.left + window.pageXOffset;

        if (left == 0) {
            left = -4;
        }

        if (left + menuOffset.width > window.innerWidth + window.pageXOffset) {
            left = window.innerWidth + window.pageXOffset - menuOffset.width + 4;
        }

        this.menu.style.left = `${left}px`;
    }
}