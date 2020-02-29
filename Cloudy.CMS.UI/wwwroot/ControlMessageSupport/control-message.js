


/* CONTROL MESSAGE */

class ControlMessage {
    constructor(target, text) {
        this.target = target;
        this.target.classList.add('cloudy-ui-has-control-message');
        this.mouseOverCallback = () => {
            this.element.style.display = 'block';
        };
        this.target.addEventListener('mouseover', this.mouseOverCallback);
        this.mouseOutCallback = () => {
            this.element.style.display = 'none';
        };
        this.target.addEventListener('mouseout', this.mouseOutCallback);

        this.element = document.createElement('cloudy-ui-control-message');
        this.element.style.display = 'none';
        document.body.appendChild(this.element);

        this.arrow = document.createElement('cloudy-ui-control-message-arrow-outer');
        this.arrow.appendChild(document.createElement('cloudy-ui-control-message-arrow-1'));
        this.arrow.appendChild(document.createElement('cloudy-ui-control-message-arrow-2'));
        this.element.appendChild(this.arrow);

        this.text = document.createElement('cloudy-ui-control-message-text');
        this.element.appendChild(this.text);
        this.text.innerText = text;

        var update = throttle(() => {
            var offset = this.target.getBoundingClientRect();

            this.element.style.left = offset.left + 'px';
            this.element.style.top = (offset.top + Math.round(offset.height)) + 'px';
        });

        this.resizeCallback = update;

        window.addEventListener('resize', this.resizeCallback);

        function setTimer() {
            update();
            setTimeout(setTimer, 1000);
        };

        setTimer();

        if (document.readyState != 'loading') {
            update();
        } else {
            document.addEventListener('DOMContentLoaded', update);
        }
    }

    remove() {
        this.target.classList.remove('cloudy-ui-has-control-message');
        this.target.removeEventListener('mouseover', this.mouseOverCallback);
        this.target.removeEventListener('mouseout', this.mouseOutCallback);
        this.element.remove();
        window.removeEventListener('resize', this.resizeCallback);
    }
}

export default ControlMessage;



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