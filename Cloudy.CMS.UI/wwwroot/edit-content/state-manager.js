const generateNewContentKey = () => (Math.random() * 0xFFFFFF << 0).toString(16).padStart(6, '0'); // https://stackoverflow.com/questions/5092808/how-do-i-randomly-generate-html-hex-color-codes-using-javascript

const arrayEquals = (a, b) => {
    if (a == null && b == null) {
        return true;
    }

    if (a == null) {
        return false;
    }

    if (b == null) {
        return false;
    }

    return a.every((ai, i) => ai === b[i]);
}

const contentReferenceEquals = (a, b) => arrayEquals(a.keys, b.keys) && a.newContentKey == b.newContentKey && a.contentTypeId == b.contentTypeId;

class StatesIndex {
    _storageKey = "cloudy:statesIndex";
    values = JSON.parse(localStorage.getItem(this._storageKey) || "[]");

    add(contentReference) {
        this.values.push(contentReference);
        localStorage.setItem(this._storageKey, JSON.stringify(this.values));
    }

    loadStates() {
        const result = [];

        for (let contentReference of this.values) {
            result.push(JSON.parse(localStorage.getItem(`cloudy:${JSON.stringify(contentReference)}`)));
        }
        
        return result;
    }
}

class StateManager {
    index = new StatesIndex();
    states = this.index.loadStates();

    createNewContent(contentType) {
        const contentReference = { newContentKey: generateNewContentKey(), keys: null, contentTypeId: contentType.id };

        this.index.add(contentReference);

        const state = {
            contentReference,
            referenceValues: {},
            referenceDate: null,
            changedFields: [],
        };
        this.states.push(state);
        this.persist(state);

        return contentReference;
    };

    getState(contentReference) {
        let state = this.states.find(s => contentReferenceEquals(s.contentReference, contentReference));



        return state;
    }

    registerChange(contentReference, change) {
        const state = this.states.find(s => contentReferenceEquals(s.contentReference, contentReference));

        let changedField = state.changedFields.find(f => arrayEquals(change.path, f.path));

        if (change.type == 'simple') {
            if (!changedField) {
                state.changedFields.push(change);
                changedField = change;
            }

            if (change.operation == 'set') {
                changedField.value = change.value;
            }
        }

        this.persist(state);

        this._onChangeCallbacks.forEach(callback => callback());
    }

    persist(state) {
        localStorage.setItem(`cloudy:${JSON.stringify(state.contentReference)}`, JSON.stringify(state));
    }

    totalChanges() {
        return this.states.reduce((accumulator, element, i) => accumulator + element.changedFields.length, 0);
    }

    _onChangeCallbacks = [];

    onChange(callback) {
        console.log('adding callback!');

        this._onChangeCallbacks.push(callback);
    }

    offChange(callback) {
        console.log('removing callback!');

        this._onChangeCallbacks.splice(this._onChangeCallbacks.indexOf(callback), 1);
    }
}

export default new StateManager();