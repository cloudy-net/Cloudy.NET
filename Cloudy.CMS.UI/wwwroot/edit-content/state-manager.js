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

class StateManager {
    states = [];

    createNewContent(contentType) {
        return { newContentKey: generateNewContentKey(), keys: null, contentTypeId: contentType.id };
    };

    getState(contentReference) {
        let state = this.states.find(s => contentReferenceEquals(s.contentReference, contentReference));

        if (!state && contentReference.newContentKey) {
            state = {
                contentReference,
                referenceValues: {},
                referenceDate: null,
                changedFields: [],
            };

            this.states.push(state);
        }

        return state;
    }

    registerChange(contentReference, change) {
        const state = this.states.find(s => contentReferenceEquals(s.contentReference, contentReference));

        let changedField = state.changedFields.find(f => arrayEquals(change.path, f.path));

        if (change.type == 'simple') {
            if (!changedField) {
                state.changedFields.push(changedField = change);
            }

            if (change.operation == 'set') {
                changedField.value = change.value;
            }
        }

        console.log(state.changedFields[0]);

    }
}

export default new StateManager();