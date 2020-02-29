


/* APP */

class App {
    constructor() {
        this.blades = [];
        this.element = document.createElement('cloudy-ui-app');
        this.element.addEventListener('cloudy-ui-close-blade', event => this.close.apply(this, [event.detail.blade, ...event.detail.parameters]));
    }

    open(blade) {
        if (!this.element.parentElement) {
            this.startBlade = blade;
            return;
        }

        this.element.appendChild(blade.element);
        this.blades.push(blade);

        blade.element.scrollIntoView({
            behavior: 'smooth',
        });

        return blade.open();
    }

    openStartBlade() {
        if (!this.startBlade) {
            return;
        }

        this.open(this.startBlade);
    }

    openAfter(blade, parentBlade) {
        return this.closeAfter(parentBlade).then(() => this.open(blade));
    }

    close(blade, ...parameters) {
        var index = this.blades.indexOf(blade);

        if (index > 1) {
            this.blades[index - 2].element.scrollIntoView({
                behavior: 'smooth',
                inline: 'start',
            });
        }

        return this.closeAfter(blade).then(() => blade.close(...parameters).then(() => {
            blade.element.remove();
            this.blades.splice(this.blades.indexOf(blade), 1);
        }));
    }

    closeAfter(blade) {
        var index = this.blades.indexOf(blade);

        if (index == this.blades.length - 1) {
            return Promise.resolve();
        }

        var blades = this.blades.slice(index + 1);

        blades.forEach((b, i) => b.element.style.zIndex = -(1 + i));

        var promises = blades
            .reverse()
            .map((b, i) => new Promise(done => {
                setTimeout(() => b.close().then(() => {
                    b.element.remove();
                    this.blades.splice(this.blades.indexOf(b), 1);
                    done();
                }), i * 200);
            }));

        return Promise.all(promises);
    }

    getBladeByElement(element) {
        return this.blades.find(b => b.element == element);
    }
}

export default App;