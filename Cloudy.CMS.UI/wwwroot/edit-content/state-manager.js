import contentGetter from "../data/content-getter.js";
import arrayEquals from "../util/array-equals.js";

const generateNewContentKey = () => (Math.random() * 0xFFFFFF << 0).toString(16).padStart(6, '0'); // https://stackoverflow.com/questions/5092808/how-do-i-randomly-generate-html-hex-color-codes-using-javascript

const contentReferenceEquals = (a, b) => arrayEquals(a.keyValues, b.keyValues) && a.newContentKey == b.newContentKey && a.contentTypeId == b.contentTypeId;

class StatesIndex {
    _storageKey = "cloudy:statesIndex";
    values = JSON.parse(localStorage.getItem(this._storageKey) || "[]");

    add(contentReference) {
        this.values.push(contentReference);
        localStorage.setItem(this._storageKey, JSON.stringify(this.values));
    }

    remove(contentReference) {
        this.values.splice(this.values.indexOf(contentReference), 1);
        localStorage.setItem(this._storageKey, JSON.stringify(this.values));
    }

    loadStates() {
        const result = [];

        for (let contentReference of this.values) {
            result.push(JSON.parse(localStorage.getItem(`cloudy:${JSON.stringify(contentReference)}`), (key, value) => key == 'referenceDate' ? new Date(value) : value));
        }
        
        return result;
    }
}

class StateManager {
    index = new StatesIndex();
    states = this.index.loadStates();

    createStateForNewContent(contentType) {
        const contentReference = { newContentKey: generateNewContentKey(), keyValues: null, contentTypeId: contentType.id };

        this.index.add(contentReference);

        const state = {
            contentReference,
            referenceValues: {},
            referenceDate: new Date(),
            changedFields: [],
        };
        this.states.push(state);
        this.persist(state);

        this.triggerAnyStateChange();
        this.triggerStateChange(contentReference);

        return contentReference;
    };

    getOrCreateStateForExistingContent(contentReference, nameHint) {
        if (this.getState(contentReference)) {
            return contentReference;
        }

        this.index.add(contentReference);

        const state = {
            contentReference,
            loading: true,
            nameHint,
            referenceValues: null,
            referenceDate: null,
            changedFields: null,
        };
        this.states.push(state);
        this.persist(state);

        this.triggerAnyStateChange();
        this.triggerStateChange(contentReference);

        contentGetter.get(contentReference).then(content => {
            const newState = {
                ...state,
                loading: false,
                nameHint: null,
                referenceValues: content,
                referenceDate: new Date(),
                changedFields: [],
            };
            this.states[this.states.indexOf(state)] = newState;
            this.persist(newState);

            this.triggerAnyStateChange();
            this.triggerStateChange(contentReference);
        });

        return contentReference;
    };

    remove(contentReference) {
        this.index.remove(contentReference);
        this.states.splice(this.states.findIndex(s => contentReferenceEquals(s.contentReference, contentReference)), 1);
        this.unpersist(contentReference);

        this.triggerAnyStateChange();
        this.triggerStateChange(contentReference);

        return contentReference;
    };

    getState(contentReference) {
        return this.states.find(s => contentReferenceEquals(s.contentReference, contentReference));
    }

    registerChange(contentReference, change) {
        const state = this.getState(contentReference);

        let changedField = state.changedFields.find(f => arrayEquals(change.path, f.path));

        if (change.type == 'simple') {
            if (!changedField) {
                state.changedFields.push(change);
                changedField = change;
            }

            if (change.operation == 'set') {
                changedField.value = change.value;
            }

            if (state.referenceValues && (state.referenceValues[change.path[0]] === null && change.value === '' || state.referenceValues[change.path[0]] == change.value)) {
                state.changedFields.splice(state.changedFields.indexOf(changedField), 1);
            }
        }

        this.persist(state);

        this.triggerAnyStateChange();
        this.triggerStateChange(contentReference);
    }

    persist(state) {
        localStorage.setItem(`cloudy:${JSON.stringify(state.contentReference)}`, JSON.stringify(state));
    }

    unpersist(contentReference) {
        localStorage.removeItem(`cloudy:${JSON.stringify(contentReference)}`);
    }

    totalChanges() {
        return this.states.filter(s => s.contentReference?.newContentKey || (s.changedFields && s.changedFields.length > 0)).length;
    }

    _onAnyStateChangeCallbacks = [];

    onAnyStateChange(callback) {
        this._onAnyStateChangeCallbacks.push(callback);
    }

    offAnyStateChange(callback) {
        this._onAnyStateChangeCallbacks.splice(this._onAnyStateChangeCallbacks.indexOf(callback), 1);
    }

    triggerAnyStateChange() {
        this._onAnyStateChangeCallbacks.forEach(callback => callback());
    }

    _onStateChangeCallbacks = {};

    onStateChange(contentReference, callback) {
        const key = JSON.stringify(contentReference);

        if (!this._onStateChangeCallbacks[key]) {
            this._onStateChangeCallbacks[key] = [];
        }

        this._onStateChangeCallbacks[key].push(callback);
    }

    offStateChange(contentReference, callback) {
        const key = JSON.stringify(contentReference);

        if (!this._onStateChangeCallbacks[key]) {
            this._onStateChangeCallbacks[key] = [];
        }

        this._onStateChangeCallbacks[key].splice(this._onStateChangeCallbacks[key].indexOf(callback), 1);
    }

    triggerStateChange(contentReference) {
        const key = JSON.stringify(contentReference);

        if (!this._onStateChangeCallbacks[key]) {
            this._onStateChangeCallbacks[key] = [];
        }

        this._onStateChangeCallbacks[key].forEach(callback => callback());
    }
}

export default new StateManager();