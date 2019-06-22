import RemoveElementListener from '../../Scripts/remove-element-listener.js';



/* CONTEXT MENU */

class ContextMenu {
    constructor() {
        this.element = document.createElement('div');

        this.element.classList.add('poetry-ui-context-menu-outer');

        var button = document.createElement('poetry-ui-context-menu-button');

        button.tabIndex = 0;

        var update = () => {
            if (!this.element.offsetParent || !this.menu.offsetParent) { // hidden or detached
                return;
            }

            this.menu.classList.remove('poetry-ui-context-menu-right');
            this.menu.classList.remove('poetry-ui-context-menu-top');

            var rectangle = this.menu.getBoundingClientRect();
            var containerRectangle = this.element.offsetParent.getBoundingClientRect();

            if (rectangle.right > containerRectangle.left + this.element.offsetParent.clientWidth) {
                this.menu.classList.add('poetry-ui-context-menu-right');
            }

            if (rectangle.bottom > containerRectangle.bottom) {
                this.menu.classList.add('poetry-ui-context-menu-top');
            }
        };

        this.updateThrottled = throttle(update, 100);

        button.addEventListener('click', () => {
            if (!button.classList.contains('poetry-ui-active')) {
                button.classList.add('poetry-ui-active');
                this.menu.style.display = '';

                update();
            } else {
                button.classList.remove('poetry-ui-active');
                this.menu.style.display = 'none';
            }
        });
        button.addEventListener("keyup", event => {
            if (event.keyCode != 13) {
                return;
            }

            event.preventDefault();
            button.click();
        });

        this.documentClickCallback = event => {
            if (event.target == button || event.target == this.menu || event.target.offsetParent == this.menu) {
                return;
            }

            button.classList.remove('poetry-ui-active');
            this.menu.style.display = 'none';
        };

        this.element.appendChild(button);

        this.menu = document.createElement('poetry-ui-context-menu');
        this.menu.style.display = 'none';

        var arrowOuter = document.createElement('div');
        arrowOuter.classList.add('poetry-ui-context-menu-arrow-outer');
        this.menu.appendChild(arrowOuter);

        var arrow1 = document.createElement('div');
        arrow1.classList.add('poetry-ui-context-menu-arrow-1');
        arrowOuter.appendChild(arrow1);

        var arrow2 = document.createElement('div');
        arrow2.classList.add('poetry-ui-context-menu-arrow-2');
        arrowOuter.appendChild(arrow2);

        this.element.appendChild(this.menu);
    }

    addItem(callback) {
        var item = new ContextMenuItem();

        callback(item);

        item.onClick(() => this.menu.style.display = 'none');

        item.appendTo(this.menu);

        return this;
    }

    clearTimer() {
        clearTimeout(this.setTimerReference);
    }

    setTimer() {
        this.updateThrottled();
        this.setTimerReference = setTimeout(() => this.setTimer(), 1000);
    }

    appendTo(element) {
        element.appendChild(this.element);

        window.addEventListener('resize', this.updateThrottled);
        document.addEventListener('click', this.documentClickCallback);
        this.setTimer();

        new RemoveElementListener(this.element, () => {
            window.removeEventListener('resize', this.updateThrottled);
            document.removeEventListener('click', this.documentClickCallback);
            this.clearTimer();
        });
    }
}

class ContextMenuItem {
    constructor() {
        this.element = document.createElement('poetry-ui-context-menu-item');
        this.element.tabIndex = 0;
        this.element.addEventListener("keyup", event => {
            if (event.keyCode != 13) {
                return;
            }

            event.preventDefault();
            this.element.click();
        });
    }

    setText(text) {
        this.element.innerText = text;

        return this;
    }

    setDisabled(value) {
        if (value) {
            this.element.setAttribute('poetry-ui-disabled', true);
            this.element.removeAttribute('tabindex');
        } else {
            this.element.removeAttribute('poetry-ui-disabled');
            this.element.tabIndex = 0;
        }
    }

    onClick(callback) {
        if (this.element.hasAttribute('poetry-ui-disabled')) {
            return;
        }

        this.element.addEventListener('click', callback);

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default ContextMenu;



/* THROTTLE */

function throttle(fn, threshhold = 250, scope) {
    var last, deferTimer;
    return function () {
        var context = scope || this;

        var now = +new Date,
            args = arguments;
        if (last && now < last + threshhold) {
            // hold on to it
            clearTimeout(deferTimer);
            deferTimer = setTimeout(function () {
                last = now;
                fn.apply(context, args);
            }, threshhold);
        } else {
            last = now;
            fn.apply(context, args);
        }
    };
}