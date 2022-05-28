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
        return this.states.filter(state => state.changes?.length);
    }

    createStateForNewContent(contentType) {
        const contentReference = { newContentKey: generateNewContentKey(), keyValues: null, contentTypeId: contentType.id };

        const state = {
            contentReference,
            referenceValues: {},
            referenceDate: new Date(),
            changes: [],
        };
        this.states.push(state);
        this.persist(state);

        this.triggerAnyStateChange();
        this.triggerStateChange(contentReference);

        return state;
    };

    createOrUpdateStateForExistingContent(contentReference, nameHint) {
        const existingState = this.getState(contentReference);
        if (existingState) {
            this.reloadContentForState(contentReference);
            return existingState;
        }

        const state = {
            contentReference,
            loading: true,
            nameHint,
            referenceValues: null,
            referenceDate: null,
            changes: null,
        };
        this.states.push(state);
        this.persist(state);

        this.triggerAnyStateChange();
        this.triggerStateChange(contentReference);

        this.loadContentForState(contentReference);

        return state;
    };

    reloadContentForState(contentReference){
        let state = this.getState(contentReference);

        state = {
            ...state,
            loadingNewVersion: true,
            newVersion: null,
        };
        this.replace(state);

        contentGetter.get(contentReference).then(content => {
            state = this.getState(contentReference);

            const newVersionIsDifferent = JSON.stringify(state.referenceValues) != JSON.stringify(state.content);

            if(JSON.stringify(state.referenceValues) == JSON.stringify(content)){
                state = {
                    ...state,
                    loadingNewVersion: false,
                };
            } else {
                if(!state.changes.length){
                    state = {
                        ...state,
                        loadingNewVersion: false,
                        referenceValues: content,
                        referenceDate: new Date(),
                    };
                } else {
                    state = {
                        ...state,
                        loadingNewVersion: false,
                        newVersion: {
                            referenceValues: content,
                            referenceDate: new Date(),
                        },
                    };
                }
            }
            
            this.replace(state);
        });
    }

    discardNewVersion(contentReference){
        let state = this.getState(contentReference);

        state = {
            ...state,
            referenceValues: state.newVersion.referenceValues,
            referenceDate: state.newVersion.referenceDate,
            newVersion: null,
        };
        this.replace(state);
    }

    loadContentForState(contentReference){
        contentGetter.get(contentReference).then(content => {
            let state = this.getState(contentReference);

            state = {
                ...state,
                loading: false,
                nameHint: null,
                referenceValues: content,
                referenceDate: new Date(),
                changes: [],
            };
            
            this.replace(state);
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

    replace(state) {
            this.states[this.states.findIndex(s => contentReferenceEquals(s.contentReference, state.contentReference))] = state;
            this.persist(state);

            this.triggerAnyStateChange();
            this.triggerStateChange(state.contentReference);
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

    registerChange(contentReference, newChange) {
        const state = this.getState(contentReference);

        let change = state.changes.find(f => arrayEquals(newChange.path, f.path));

        if (newChange.type == 'simple') {
            if (!change) {
                state.changes.push(newChange);
                change = newChange;
            }

            if (newChange.operation == 'set') {
                change.value = newChange.value;
            }

            if (state.referenceValues && ((state.referenceValues[newChange.path[0]] === null || state.referenceValues[newChange.path[0]] === undefined) && newChange.value === '' || state.referenceValues[newChange.path[0]] == newChange.value)) {
                state.changes.splice(state.changes.indexOf(change), 1);
            }
        }
        
        if (newChange.type == 'array') {
            if (!change) {
                state.changes.push(newChange);
                change = newChange;
            }
        }

        this.persist(state);

        this.triggerAnyStateChange();
        this.triggerStateChange(contentReference);
    }

    discardChanges(contentReference, change) {
        const state = this.getState(contentReference);

        state.changes.splice(0, state.changes.length);

        this.persist(state);

        this.triggerAnyStateChange();
        this.triggerStateChange(contentReference);
    }

    updateIndex(){
        localStorage.setItem(this.indexStorageKey, JSON.stringify(this.states.filter(state => state.changes?.length).map(state => state.contentReference)));
    }
    
    persist(state) {
        if(state.changes?.length){
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