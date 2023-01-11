import contentGetter from "./content-getter.js";
import arrayEquals from "../util/array-equals.js";
import getReferenceValue from "../util/get-reference-value.js";
import contentSaver from "./content-saver.js";
import hasChanges from './has-changes.js';
import simpleChangeHandler from "./change-handlers/simple-change-handler.js";

const generateNewContentKey = () => (Math.random() * 0xFFFFFF << 0).toString(16).padStart(6, '0'); // https://stackoverflow.com/questions/5092808/how-do-i-randomly-generate-html-hex-color-codes-using-javascript

const contentReferenceEquals = (a, b) => arrayEquals(a.keyValues, b.keyValues) && a.newContentKey == b.newContentKey && a.entityType == b.entityType;

class StateManager {
    indexStorageKey = "cloudy:statesIndex";
    schema = "1.3";
    states = this.loadStates();
    handlers = [simpleChangeHandler]

    loadStates() {
        let index = JSON.parse(localStorage.getItem(this.indexStorageKey) || JSON.stringify({ schema: this.schema, elements: [] }));

        if (index.schema != this.schema) {
            if (confirm(`Warning: The state schema has changed (new version: ${this.schema}, old version: ${index.schema}).\n\nThis means the format of local state has changed, and your local changes are no longer understood by the Admin UI.\n\nYou are required to clear your local changes to avoid any strange bugs.\n\nPress OK to continue, or cancel to do the necessary schema changes manually to your localStorage (not supported officially).`)) {
                Object.keys(localStorage)
                    .filter(key => key.startsWith("cloudy:"))
                    .forEach(key => localStorage.removeItem(key));

                return [];
            }
        }

        const result = [];

        for (let contentReference of index.elements) {
            result.push(JSON.parse(localStorage.getItem(`cloudy:${JSON.stringify(contentReference)}`), (key, value) => key == 'referenceDate' && value ? new Date(value) : value));
        }

        return result;
    }

    getAll() {
        return this.states.filter(state => hasChanges(state));
    }

    createStateForNewContent(entityType) {
        const contentReference = { newContentKey: generateNewContentKey(), keyValues: null, entityType };

        const state = {
            contentReference,
            referenceValues: {},
            referenceDate: new Date(),
            arrayChanges: [],
        };
        this.handlers.forEach(h => h.initState(state));
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
            arrayChanges: null,
        };
        this.handlers.forEach(h => h.nullState(state));
        this.states.push(state);
        this.persist(state);

        this.triggerAnyStateChange();
        this.triggerStateChange(contentReference);

        this.loadContentForState(contentReference);

        return state;
    };

    reloadContentForState(contentReference) {
        let state = this.getState(contentReference);

        state = {
            ...state,
            loadingNewVersion: true,
            newVersion: null,
        };
        this.replace(state);

        contentGetter.get(contentReference).then(content => {
            state = this.getState(contentReference);

            if (JSON.stringify(state.referenceValues) == JSON.stringify(content)) {
                state = {
                    ...state,
                    loadingNewVersion: false,
                };
            } else {
                if (!hasChanges(state)) {
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

    discardNewVersion(contentReference) {
        let state = this.getState(contentReference);

        state = {
            ...state,
            referenceValues: state.newVersion.referenceValues,
            referenceDate: state.newVersion.referenceDate,
            newVersion: null,
        };
        this.replace(state);
    }

    async loadContentForState(contentReference) {
        const content = await contentGetter.get(contentReference);

        let state = this.getState(contentReference);

        state = {
            ...state,
            loading: false,
            nameHint: null,
            referenceValues: content,
            referenceDate: new Date(),
            arrayChanges: [],
        };
        this.handlers.forEach(h => h.initState(state));

        this.replace(state);
    }

    async save(contentReferences) {
        const results = await contentSaver.save(contentReferences.map(c => this.getState(c)));

        for (let result of results.filter(r => r.success)) {
            this.loadContentForState(result.contentReference);
        }
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

    registerArrayAdd(contentReference, path, key, type) {
        const state = this.getState(contentReference);

        const arrayAdd = { path, key, type };
        state.arrayChanges.push(arrayAdd);

        this.persist(state);

        this.triggerAnyStateChange();
        this.triggerStateChange(contentReference);
    }

    discardChanges(contentReference, change) {
        const state = this.getState(contentReference);

        state.arrayChanges.splice(0, state.arrayChanges.length);
        this.handlers.forEach(h => h.discardChanges(state));

        this.persist(state);

        this.triggerAnyStateChange();
        this.triggerStateChange(contentReference);
    }

    updateIndex() {
        localStorage.setItem(this.indexStorageKey, JSON.stringify({ schema: this.schema, elements: this.states.filter(state => hasChanges(state)).map(state => state.contentReference) }));
    }

    persist(state) {
        if (state.arrayChanges?.length || this.handlers.filter(h => h.hasChanges(state)).length) {
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