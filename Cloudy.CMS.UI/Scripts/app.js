


/* APP */

class App {
    constructor() {
        this.blades = [];
        this.element = document.createElement('poetry-ui-app');
        this.element.addEventListener('poetry-ui-close-blade', event => this.closeBlade.apply(this, [event.detail.blade, ...event.detail.parameters]));
    }

    open() {

    }

    openBlade(blade, parentBlade) {
        if (parentBlade instanceof HTMLElement) {
            parentBlade = this.blades.find(b => b.element.contains(parentBlade));
        }

        if (parentBlade && this.blades.indexOf(parentBlade) == -1) {
            throw 'Blade not found';
        }

        var done;
        var promise = new Promise(resolve => done = resolve);

        var open = () => {
            this.element.appendChild(blade.element);

            blade.element.classList.add('poetry-ui-hidden');
            blade.element.getBoundingClientRect(); // force reflow

            if (parentBlade) {
                parentBlade.element.scrollIntoView({
                    behavior: 'smooth',
                    inline: 'start',
                });
            }

            blade.element.classList.remove('poetry-ui-hidden');
            blade.element.style.zIndex = -1;

            blade.element.addEventListener('transitionend', callback);

            function callback() {
                blade.element.style.zIndex = '';

                blade.element.removeEventListener('transitionend', callback);

                done();
            }

            this.blades.push(blade);

            done();
        }

        var bladesAfterParentBlade = parentBlade ? this.blades.slice(this.blades.indexOf(parentBlade) + 1) : [];

        if (bladesAfterParentBlade.length) {
            bladesAfterParentBlade.forEach(blade => {
                blade.triggerOnClose();
                blade.element.remove();

                this.blades.splice(this.blades.indexOf(blade), 1);
            });

            this.element.appendChild(blade.element);
            this.blades.push(blade);

            done();
        } else {
            open();
        }

        return promise;
    }

    closeBlade(arg, ...parameters) {
        if (!arg) {
            throw 'No blade specified';
        }

        var index = this.blades.indexOf(arg);

        if (index == -1) {
            index = this.blades.findIndex(b => b.element == arg);
        }

        if (index == -1) {
            throw 'Blade not found';
        }

        var scrollToBlade = this.blades[index - 2] || this.blades[index - 1];

        if (scrollToBlade) {
            scrollToBlade.element.scrollIntoView({
                behavior: 'smooth',
                inline: 'start',
            });
        }

        var done;
        var promise = new Promise(resolve => done = resolve);

        var blades = this.blades.slice(index);

        blades.forEach((blade, i) => blade.element.style.zIndex = -(1 + i));

        blades
            .reverse()
            .forEach((blade, i) => {
                setTimeout(() => {
                    blade.element.classList.add('poetry-ui-hidden');

                    blade.element.addEventListener('transitionend', () => {
                        if (i == blades.length - 1) {
                            blade.triggerOnClose(...parameters);
                            done();
                        } else {
                            blade.triggerOnClose();
                        }

                        blade.element.remove();
                    });

                }, i * 200);

                this.blades.splice(this.blades.indexOf(blade), 1);
            });

        if (index == 0) {
            promise.then(() => this.element.remove());
        }

        return promise;
    }
}

export default App;