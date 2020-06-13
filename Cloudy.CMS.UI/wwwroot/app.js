


/* APP */

class App {
    constructor() {
        this.blades = [];
        this.element = document.createElement('cloudy-ui-app');
        this.element.addEventListener('cloudy-ui-close-blade', event => this.removeBlade.apply(this, [event.detail.blade, ...event.detail.parameters]));
    }

    async open() {
    }

    async close() {
    }

    async addBlade(blade) {
        if (!this.element.parentElement) {
            this.startBlade = blade;
            return;
        }

        this.element.appendChild(blade.element);
        this.blades.push(blade);

        blade.element.scrollIntoView({
            behavior: 'smooth',
        });

        return await blade.open();
    }

    addBladeAfter(blade, parentBlade) {
        return this.removeBladeAfter(parentBlade).then(() => this.addBlade(blade));
    }

    async removeBlade(blade, ...parameters) {
        var index = this.blades.indexOf(blade);

        if (index > 1) {
            this.blades[index - 2].element.scrollIntoView({
                behavior: 'smooth',
                inline: 'start',
            });
        }

        await this.removeBladeAfter(blade);
        await blade.close(...parameters);

        blade.element.remove();
        this.blades.splice(this.blades.indexOf(blade), 1);
    }

    async removeBladeAfter(blade) {
        var index = this.blades.indexOf(blade);

        if (index == this.blades.length - 1) {
            return Promise.resolve();
        }

        var blades = this.blades.slice(index + 1);

        blades.forEach((b, i) => b.element.style.zIndex = -(1 + i));

        await Promise.all(blades
            .reverse()
            .map((b, i) => new Promise(done => setTimeout(
                async () => {
                    await b.close();
                    b.element.remove();
                    this.blades.splice(this.blades.indexOf(b), 1);
                    done();
                },
                i * 200
            ))));
    }
}

export default App;