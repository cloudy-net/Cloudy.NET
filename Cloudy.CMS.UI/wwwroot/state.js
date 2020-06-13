class State {
    segments = [];

    constructor() {
        this.update();
    }

    getFor(blade) {
        return this.segments[blade.getIndex() + 1];
    }

    update() {
        if (!location.hash) {
            this.segments = [];

            return;
        }

        this.segments = location.hash.substr(1).split('/');
    }

    set(value, blade) {
        var index = blade ? blade.getIndex() : 0
        var segments = [...this.segments.slice(0, index + 1), value];
        var hash = `#${segments.join('/')}`;

        if (location.hash == hash) {
            return;
        }

        location.href = hash;
    }

    pop(blade) {
        this.segments = this.segments.slice(0, blade.getIndex());
        location.href = `#${this.segments.join('/')}`;
    }
}

export default new State();