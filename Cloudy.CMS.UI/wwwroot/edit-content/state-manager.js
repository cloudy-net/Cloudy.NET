import contentGetter from "../data/content-getter.js";
import arrayEquals from "../util/array-equals.js";
import contentSaver from "./content-saver.js";

const generateNewContentKey = () => (Math.random() * 0xFFFFFF << 0).toString(16).padStart(6, '0'); // https://stackoverflow.com/questions/5092808/how-do-i-randomly-generate-html-hex-color-codes-using-javascript

const contentReferenceEquals = (a, b) => arrayEquals(a.keyValues, b.keyValues) && a.newContentKey == b.newContentKey && a.contentTypeId == b.contentTypeId;

class StateManager {
    indexStorageKey = "cloudy:statesIndex";
    states = this.loadStates();

    loadStates() {
        const index = JSON.parse(localStorage.getItem(this.indexStorageKey) || "[]");

        const result = [];

        for (let contentReference of index) {
            result.push(JSON.parse(localStorage.getItem(`cloudy:${JSON.stringify(contentReference)}`), (key, value) => key == 'referenceDate' && value ? new Date(value) : value));
        }
        
        return result;
    }

    getAll(){
        return this.states.filter(state => state.changedFields?.length);
    }

    createStateForNewContent(contentType) {
        const contentReference = { newContentKey: generateNewContentKey(), keyValues: null, contentTypeId: contentType.id };

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
        const existingState = this.getState(contentReference);
        if (existingState) {
            return existingState.contentReference;
        }

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

        this.loadStateForContent(contentReference);

        return contentReference;
    };

    loadStateForContent(contentReference){
        contentGetter.get(contentReference).then(content => {
            const state = this.getState(contentReference);
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
    }

    save(contentReferences){
        contentSaver
            .save(contentReferences.map(c => this.getState(c)))
            .then(results => {
                for(let result of results.filter(r => r.success)){
                    this.loadStateForContent(result.contentReference);
                }
            });
    }

    remove(contentReference) {
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

    updateIndex(){
        localStorage.setItem(this.indexStorageKey, JSON.stringify(this.states.filter(state => state.changedFields?.length).map(state => state.contentReference)));
    }
    
    persist(state) {
        if(state.changedFields?.length){
            localStorage.setItem(`cloudy:${JSON.stringify(state.contentReference)}`, JSON.stringify(state));
        } else {
            localStorage.removeItem(`cloudy:${JSON.stringify(state.contentReference)}`);
        }
        this.updateIndex();
    }

    unpersist(contentReference) {
        localStorage.removeItem(`cloudy:${JSON.stringify(contentReference)}`);
        this.updateIndex();
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